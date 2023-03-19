namespace CS_Utils.Helpers;
// We should keep the generator as a singleton, it means that we should only create the generator
// once. If not, it may generate some duplicate Ids.
public class SnowflakeIdGenerator
{
    public const int TimestampLeftShift = _sequenceBits + _workerIdBits + _datacenterIdBits;
    public const long Twepoch = 1288834974000L;
    private const int _datacenterIdBits = 5;
    private const int _datacenterIdShift = _sequenceBits + _workerIdBits;
    private const long _maxDatacenterId = -1L ^ (-1L << _datacenterIdBits);
    private const long _maxWorkerId = -1L ^ (-1L << _workerIdBits);
    private const int _sequenceBits = 12;
    private const long _sequenceMask = -1L ^ (-1L << _sequenceBits);
    private const int _workerIdBits = 5;
    private const int _workerIdShift = _sequenceBits;
    private readonly object _lock = new();
    private long _lastTimestamp = -1L;

    public SnowflakeIdGenerator(long workerId, long datacenterId, long sequence = 0L)
    {
        if (workerId > _maxWorkerId || workerId < 0)
            throw new ArgumentException($"worker Id must greater than or equal 0 and less than or equal {_maxWorkerId}");

        if (datacenterId > _maxDatacenterId || datacenterId < 0)
            throw new ArgumentException(
                $"datacenter Id must greater than or equal 0 and less than or equal {_maxDatacenterId}");

        WorkerId = workerId;
        DatacenterId = datacenterId;
        Sequence = sequence;
    }

    public long DatacenterId { get; protected set; }

    public long Sequence { get; internal set; }

    public long WorkerId { get; protected set; }

    public long NextId()
    {
        lock (_lock)
        {
            var timestamp = TimeGen();
            if (timestamp < _lastTimestamp) throw new Exception("timestamp error");

            if (_lastTimestamp == timestamp)
            {
                Sequence = (Sequence + 1) & _sequenceMask;

                if (Sequence == 0) timestamp = TilNextMillis(_lastTimestamp);
            }
            else
                Sequence = 0;

            _lastTimestamp = timestamp;
            return ((timestamp - Twepoch) << TimestampLeftShift) | (DatacenterId << _datacenterIdShift) |
                   (WorkerId << _workerIdShift) | Sequence;
        }
    }

    private long TilNextMillis(long lastTimestamp)
    {
        var timestamp = TimeGen();
        while (timestamp <= lastTimestamp) timestamp = TimeGen();

        return timestamp;
    }

    private long TimeGen()
    {
        return DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
    }
}