using System.Threading.Tasks;

namespace Bootstrap.Client.Extensions
{
    /// <summary>
    /// ISendMail 接口
    /// </summary>
    public interface ISendMail
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="format"></param>
        /// <param name="mailBody"></param>
        /// <returns></returns>
        Task<bool> SendMailAsync(MessageFormat format, string mailBody);
    }
}
