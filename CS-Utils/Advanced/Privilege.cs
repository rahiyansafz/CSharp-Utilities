﻿namespace CS_Utils.Advanced;
public abstract class Privilege<TItem, TUser>
{
    public abstract bool CanCreate(TItem item, TUser user);

    public abstract bool CanDelete(TItem item, TUser user);

    public abstract bool CanEdit(TItem item, TUser user);

    public abstract bool CanView(TItem item, TUser user);
}