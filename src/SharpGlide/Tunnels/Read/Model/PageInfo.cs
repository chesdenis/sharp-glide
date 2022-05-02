﻿namespace SharpGlide.Tunnels.Read.Model
{
    public struct PageInfo
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }

        public static PageInfo Default => new PageInfo();
    }
}