using System.Collections.Generic;
using System.Text;

namespace Duo1JFramework.Asset
{
    /// <summary>
    /// 资源检查结果
    /// </summary>
    public class AssetCheckResult
    {
        private readonly List<string> infoReasons = new List<string>();
        private readonly List<string> errorReasons = new List<string>();

        public bool IsValid => errorReasons.Count == 0;

        public IReadOnlyList<string> InfoReasons => infoReasons;

        public IReadOnlyList<string> ErrorReasons => errorReasons;

        public void AddInfo(string reason)
        {
            if (!string.IsNullOrEmpty(reason))
            {
                infoReasons.Add(reason);
            }
        }

        public void AddError(string reason)
        {
            if (!string.IsNullOrEmpty(reason))
            {
                errorReasons.Add(reason);
            }
        }

        public string GetMessage()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(IsValid ? "资源检查通过" : "资源检查未通过：");
            AppendReasons(sb, "普通信息", infoReasons);
            AppendReasons(sb, "报错信息", errorReasons);
            return sb.ToString();
        }

        private static void AppendReasons(StringBuilder sb, string title, IReadOnlyList<string> reasons)
        {
            if (reasons.Count <= 0)
            {
                return;
            }

            sb.AppendLine();
            sb.AppendLine(title + "：");
            for (int i = 0; i < reasons.Count; i++)
            {
                sb.AppendLine($"{i + 1}. {reasons[i]}");
            }
        }
    }
}
