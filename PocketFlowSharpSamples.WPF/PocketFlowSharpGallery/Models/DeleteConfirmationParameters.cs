namespace PocketFlowSharpGallery.Models
{
    /// <summary>
    /// 删除确认对话框参数
    /// </summary>
    public class DeleteConfirmationParameters
    {
        /// <summary>
        /// 对话框标题
        /// </summary>
        public string Title { get; set; } = "确认删除";

        /// <summary>
        /// 项目类型描述（如"配置"、"用户"、"文件"等）
        /// </summary>
        public string ItemType { get; set; } = "项目";

        /// <summary>
        /// 项目名称
        /// </summary>
        public string ItemName { get; set; } = string.Empty;

        /// <summary>
        /// 警告消息
        /// </summary>
        public string WarningMessage { get; set; } = "此操作不可撤销，删除后将无法恢复。";

        /// <summary>
        /// 确认按钮文本
        /// </summary>
        public string ConfirmButtonText { get; set; } = "删除";

        /// <summary>
        /// 取消按钮文本
        /// </summary>
        public string CancelButtonText { get; set; } = "取消";
    }
}