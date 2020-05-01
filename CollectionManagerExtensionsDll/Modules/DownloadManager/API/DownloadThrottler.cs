using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace CollectionManagerExtensionsDll.Modules.DownloadManager.API
{
    public class DownloadThrottler
    {
        public int DownloadsPerMinute { get; set; }
        public int DownloadsPerHour { get; set; }
        public List<DateTime> DownloadTimes { get; } = new List<DateTime>();

        public DownloadThrottler(int downloadsPerMinute, int downloadsPerHour)
        {
            DownloadsPerMinute = downloadsPerMinute;
            DownloadsPerHour = downloadsPerHour;
        }

        public void Reset()
        {
            DownloadTimes.Clear();
        }

        public void RegisterDownload()
        {
            DownloadTimes.Add(DateTime.UtcNow);
        }

        public string GetStatus()
        {
            var downloadedInLastMinute = RecentDownloadsCount(TimeSpan.FromMinutes(1));
            var downloadedInLastHour = RecentDownloadsCount(TimeSpan.FromHours(1));
            var limitedByMinute = downloadedInLastMinute >= DownloadsPerMinute;
            var limitedByHour = downloadedInLastHour >= DownloadsPerHour;
            DateTime? nextSlotAvaliableAt = null;
            if (limitedByHour)
            {
                var downloadsOverLimit = downloadedInLastHour - DownloadsPerHour;
                if (downloadsOverLimit >= 0)
                {
                    nextSlotAvaliableAt = RecentDownloadTimes(TimeSpan.FromHours(1)).OrderByDescending(x => x.Date).Last().AddHours(1);
                }
            }
            else if (limitedByMinute)
            {
                var downloadsOverLimit = downloadedInLastMinute - DownloadsPerMinute;
                if (downloadsOverLimit >= 0)
                {
                    nextSlotAvaliableAt = RecentDownloadTimes(TimeSpan.FromMinutes(1)).OrderByDescending(x => x.Date).Last().AddMinutes(1);
                }
            }

            if (nextSlotAvaliableAt.HasValue)
                return $"Next download slot avaliable at {nextSlotAvaliableAt.Value.ToLocalTime()}";

            return "Download slot available";
        }

        public virtual bool CanDownload()
        {
            var downloadedInLastMinute = RecentDownloadsCount(TimeSpan.FromMinutes(1));
            if (DownloadsPerMinute <= downloadedInLastMinute)
            {
                return false;
            }

            var downloadedInLastHour = RecentDownloadsCount(TimeSpan.FromHours(1));
            if (DownloadsPerHour <= downloadedInLastHour)
            {
                return false;
            }


            return true;
        }

        public int RecentDownloadsCount(TimeSpan timeSpan)
            => RecentDownloadTimes(timeSpan).Count();

        public IEnumerable<DateTime> RecentDownloadTimes(TimeSpan timeSpan)
        {
            var since = DateTime.UtcNow.Subtract(timeSpan);
            return DownloadTimes.Where(x => x >= since);
        }

    }
}