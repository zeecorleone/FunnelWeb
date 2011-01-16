﻿using System.Collections.Generic;
using FunnelWeb.Model;

namespace FunnelWeb.Web.Views.Wiki
{
    public class RecentModel
    {
        public RecentModel(IEnumerable<Entry> revisions, int pageNumber, int totalPages, string actionName)
        {
            Entries = revisions;
            PageNumber = pageNumber;
            TotalPages = totalPages;
            ActionName = actionName;
        }

        public IEnumerable<Entry> Entries { get; set; }
        public int PageNumber { get; set; }
        public int TotalPages { get; set; }
        public string ActionName { get; set; }
    }
}