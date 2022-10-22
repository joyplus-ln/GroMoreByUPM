
//------------------------------------------------------------------------------
// Copyright (c) 2018-2019 Beijing Bytedance Technology Co., Ltd.
// All Right Reserved.
// Unauthorized copying of this file, via any medium is strictly prohibited.
// Proprietary and confidential.
//------------------------------------------------------------------------------

namespace ByteDance.Union
{
#if UNITY_EDITOR || (!UNITY_ANDROID && !UNITY_IOS)
    /// <summary>
    /// The slot of a advertisement.
    /// </summary>
    using UnityEngine;
    using System.Collections.Generic;
    public sealed class ABUUserInfoForSegment
    {
        // 用户ID
        public string userID;
        // 渠道
        public string channel;
        // 子渠道
        public string subChannel;
        // 年龄
        public int age;
        // 用户分组
        public string userGroup;
        // 性别
        public ABUUserInfoGender gender;
        // 自定义信息
        public Dictionary<string, string> customDataDictionary;

        public static AndroidJavaObject getCurrentAndroidObject()
        {
            return null;
        }

        public static ABUUserInfoForSegment CreateSharedSegment()
        {
            return null;
        }

        /// <summary>
        /// The builder used to build an ABUUserInfoForSegment.
        /// </summary>
        public class Builder
        {
            /// <summary>
            /// Sets the user ID.
            /// </summary>
            public Builder SetUserID(string userID)
            {
                return this;
            }

            /// <summary>
            /// Sets channel.
            /// </summary>
            public Builder SetChannel(string channel)
            {
                return this;
            }

            /// <summary>
            /// Sets the sub channel.
            /// </summary>
            public Builder SetSubChannel(string subChannel)
            {
                return this;
            }

            /// <summary>
            /// Sets the age.
            /// </summary>
            public Builder SetAge(int age)
            {
                return this;
            }

            /// <summary>
            /// Sets a value : userGroup
            /// </summary>
            public Builder SetUserGroup(string userGroup)
            {
                return this;
            }

            /// <summary>
            /// Sets the gender.
            /// </summary>
            public Builder SetGender(ABUUserInfoGender gender)
            {
                return this;
            }

            /// <summary>
            /// Sets the custom data dictionary.
            /// </summary>
            /// <returns>The custom data dictionary.</returns>
            /// <param name="dictionary">Dictionary.</param>
            public Builder setCustomDataDictionary(Dictionary<string, string> dictionary)
            {
                return this;
            }

            /// <summary>
            /// Build the ABUUserInfoForSegment.
            /// </summary>
            public ABUUserInfoForSegment Build()
            {
                return new ABUUserInfoForSegment();
            }
        }
    }
#endif
}
