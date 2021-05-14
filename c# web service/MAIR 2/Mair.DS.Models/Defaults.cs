namespace Mair.DS.Models
{
    /// <summary>
    /// 
    /// </summary>
    public static class Defaults
    {
        #region Fields and Properties

        /// <summary>
        /// Gets the name of the connection string.
        /// </summary>
        /// <value>
        /// The name of the connection string.
        /// </value>
        public static string ConnectionString { get; set; }

        #endregion

        #region Constructors

        static Defaults()
        {
            ConnectionString = "";
        }

        #endregion

        #region Methods
        #endregion
    }
}
