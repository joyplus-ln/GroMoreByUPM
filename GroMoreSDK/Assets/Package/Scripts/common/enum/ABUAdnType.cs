namespace ByteDance.Union
{
    /// <summary>
    /// 广告Adn类型
    /// Login type.
    /// </summary>
    public enum ABUAdnType
    {
        ABUAdnNoPermission = -3,    // 无权限访问
        ABUAdnNoData = -2,  // 暂时无真实数据，未获取到最佳广告，一般在未展示之前
        ABUAdnNone = 0,     // 未知adn
        ABUAdnPangle = 1,   // 穿山甲adn
        ABUAdnAdmob = 2,    // 谷歌Admob
        ABUAdnGDT = 3,      // 腾讯广点通adn
        ABUAdnMTG = 4,     // Mintegral adn
        ABUAdnUnity = 5,   // unity adn
        ABUAdnBaidu = 6,   // 百度adn
        ABUAdnKs = 7,      // 快手Adn
        ABUAdnSigmob = 8,  // Sigmob adn
        ABUAdnKlevin = 9   // 游可赢
    }
}
