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
                    case Status.Pending:
                        _StatusNames.Add(MonStatus, new Dictionary<string, string>() { { "EN", "Pending" }, { "FR", "En attente de validation" } });
                        break;
                    case Status.Validated:
                        _StatusNames.Add(MonStatus, new Dictionary<string, string>() { { "EN", "Validated" }, { "FR", "Validé" } });
                        break;
                    case Status.Scrapped:
                        _StatusNames.Add(MonStatus, new Dictionary<string, string>() { { "EN", "Scrapped" }, { "FR", "Détruit" } });
                        break;
                    case Status.Consummed:
                        _StatusNames.Add(MonStatus, new Dictionary<string, string>() { { "EN", "Consummed" }, { "FR", "Consommé" } });
                        break;
                    case Status.Sent:
                        _StatusNames.Add(MonStatus, new Dictionary<string, string>() { { "EN", "Sent" }, { "FR", "Envoyé" } });
                        break;
                    case Status.StoredExternally:
                        _StatusNames.Add(MonStatus, new Dictionary<string, string>() { { "EN", "StoredExternally" }, { "FR", "Stocké à l'extérieur" } });
                        break;
                    case Status.Exit:
                        _StatusNames.Add(MonStatus, new Dictionary<string, string>() { { "EN", "Exit" }, { "FR", "Sorti" } });
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
