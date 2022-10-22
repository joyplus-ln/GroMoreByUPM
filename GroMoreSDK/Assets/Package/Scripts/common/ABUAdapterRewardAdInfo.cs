
//------------------------------------------------------------------------------
// Copyright (c) 2018-2019 Beijing Bytedance Technology Co., Ltd.
// All Right Reserved.
// Unauthorized copying of this file, via any medium is strictly prohibited.
// Proprietary and confidential.
//------------------------------------------------------------------------------

using Newtonsoft.Json;

namespace ByteDance.Union
{
    /// <summary>
    /// 奖励发放验证信息.
    /// </summary>
    public sealed class ABUAdapterRewardAdInfo
    {
        [JsonProperty("rewardName")]
        public string rewardName; // 发放奖励的名称
        [JsonProperty("rewardAmount")]
        public int rewardAmount; // 发放奖励的金额
        [JsonProperty("tradeId")]
        public string tradeId; // 交易的唯一标识
        [JsonProperty("verify")]
        public bool verify; // 是否验证通过
        [JsonProperty("verifyByGroMoreS2S")]
        public bool verifyByGroMoreS2S; // 是否是通过GroMore的S2S的验证
        [JsonProperty("adnName")]
        public string adnName; // 验证奖励发放的媒体名称，官方支持的ADN名称详见`ABUAdnType`注释部分，自定义ADN名称同平台配置
        [JsonProperty("reason")]
        public string reason; // 验证失败的原因
        [JsonProperty("errorCode")]
        public int errorCode; // 无法完成验证的错误码
        [JsonProperty("errorMsg")]
        public string errorMsg; // 无法完成验证的错误原因，包括网络错误、服务端无响应、服务端无法验证等
        [JsonProperty("rewardType")]
        public int rewardType; // 奖励类型，0:基础奖励 1:进阶奖励-互动 2:进阶奖励-超过30s的视频播放完成  目前支持返回该字段的adn：csj
        [JsonProperty("rewardPropose")]
        public float rewardPropose; // 建议奖励百分比， 基础奖励为1，进阶奖励为0.0 ~ 1.0，开发者自行换算  目前支持返回该字段的adn：csj

        public class Builder
        {
            public ABUAdapterRewardAdInfo Build() => new ABUAdapterRewardAdInfo();
            public Builder SetRewardName(string rewardName) => this;
            public Builder SetRewardAmount(int rewardAmount) => this;
            public Builder SetVerify(bool verify) => this;
            public Builder SetVerifyByGroMoreS2S(bool verifyByGroMoreS2S) => this;
            public Builder SetAdnName(string adnName) => this;
            public Builder SetReason(string reason) => this;
            public Builder SetRrrorCode(int errorCode) => this;
            public Builder SetRrrorMsg(string errorMsg) => this;
            public Builder SetRewardType(int rewardType) => this;
            public Builder SetRewardPropose(float rewardPropose) => this;
        }
    }
}
