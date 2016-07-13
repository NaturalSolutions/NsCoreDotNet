using CommonDynPropInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonDynPropManager
{
    /// <summary>
    /// Class managing status description
    /// </summary>
    public static class StatusManager
    {
        /// <summary>
        /// List of status Name
        /// </summary>
        public static Dictionary<Status, Dictionary<string, string>> _StatusNames = new Dictionary<Status, Dictionary<string, string>>();


        /// <summary>
        /// List of status Name
        /// </summary>
        public static Dictionary<Status, Dictionary<string, string>> StatusNames
        {
            get
            {
                if (_StatusNames.Count == 0)
                {
                    lock (_StatusNames)
                    {
                        LoadStatusNames();
                    }
                }
                return _StatusNames;
            }
        }



        /// <summary>
        /// Load status name from configuration
        /// </summary>
        public static void LoadStatusNames()
        {
            _StatusNames = new Dictionary<Status, Dictionary<string, string>>();

            foreach (Status MonStatus in (Status[])Enum.GetValues(typeof(Status)))
            {

                switch (MonStatus)
                {
                    case Status.ToBeValidated:
                        _StatusNames.Add(MonStatus, new Dictionary<string, string>() { { "EN", "Pending" } });
                        break;
                    case Status.Deleted:
                        _StatusNames.Add(MonStatus, new Dictionary<string, string>() { { "EN", "Scrapped" } });
                        break;
                    case Status.Outside:
                        _StatusNames.Add(MonStatus, new Dictionary<string, string>() { { "EN", "Stored externally" } });
                        break;
                    default:
                        _StatusNames.Add(MonStatus, new Dictionary<string, string>() { { "EN", MonStatus.ToString() } });
                        break;
                }
            }

        }

        /// <summary>
        /// Get the name of one status on the given language
        /// </summary>
        /// <param name="MonStatus">Status value</param>
        /// <param name="Language">Language</param>
        /// <returns>string for status in appropriates language</returns>
        public static string GetStatusName(Status MonStatus, string Language)
        {
            return StatusNames[MonStatus][Language];

        }



    }
}
