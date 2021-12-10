namespace Bootstrap.DataAccess
{
    /// <summary>
    /// 
    /// </summary>
    public class MessageBody
    {
        /// <summary>
        /// 
        /// </summary>
        public string Message { get; set; } = "";
        /// <summary>
        /// 
        /// </summary>
        public string Category { get; set; } = "";
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0}-{1}", Category, Message);
        }
    }
}
