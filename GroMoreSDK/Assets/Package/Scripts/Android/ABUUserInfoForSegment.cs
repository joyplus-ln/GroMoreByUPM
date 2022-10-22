
//------------------------------------------------------------------------------
// Copyright (c) 2018-2019 Beijing Bytedance Technology Co., Ltd.
// All Right Reserved.
// Unauthorized copying of this file, via any medium is strictly prohibited.
// Proprietary and confidential.
//------------------------------------------------------------------------------

namespace ByteDance.Union
{
//#if UNITY_ANDROID
#if !UNITY_EDITOR && UNITY_ANDROID
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
        public bool age;
        // 用户分组
        public string userGroup;
        // 性别
        public ABUUserInfoGender gender;
        // 自定义信息，暂时未用到
        public string customInfos;
        public Dictionary<string, string> customDataDictionary;

        private static AndroidJavaObject userInfosg;

        public static AndroidJavaObject getCurrentAndroidObject()
        {
            if(userInfosg == null) {
                userInfosg = new AndroidJavaObject("com.bytedance.msdk.api.UserInfoForSegment");
            }
            return userInfosg; 
        }

        /// <summary>
        /// The builder used to build an ABUUserInfoForSegment.
        /// </summary>
        public class Builder
        {
            /// <summary>
            /// Sets the user ID.
            /// </summary>
            public Builder SetUserID(string userId)
            {
                ABUUserInfoForSegment.getCurrentAndroidObject().Call("setUserId", userId);
                return this;
            }

            /// <summary>
            /// Sets channel.
            /// </summary>
            public Builder SetChannel(string channel)
            {
                ABUUserInfoForSegment.getCurrentAndroidObject().Call("setChannel", channel);
                return this;
            }

            /// <summary>
            /// Sets the sub channel.
            /// </summary>
            public Builder SetSubChannel(string subChannel)
            {
                ABUUserInfoForSegment.getCurrentAndroidObject().Call("setSubChannel", subChannel);
                return this;
            }

            /// <summary>
            /// Sets the age.
            /// </summary>
            public Builder SetAge(int age)
            {
                ABUUserInfoForSegment.getCurrentAndroidObject().Call("setAge", age);
                return this;
            }

            /// <summary>
            /// Sets a value : userGroup
            /// </summary>
            public Builder SetUserGroup(string userGroup)
            {
                ABUUserInfoForSegment.getCurrentAndroidObject().Call("setUserValueGroup", userGroup);
                return this;
            }

            /// <summary>
            /// Sets the gender.
            /// </summary>
            public Builder SetGender(ABUUserInfoGender gender)
            {
                string genderinfo = "unknown";
                if (gender == ABUUserInfoGender.ABUUserInfoGenderFemale)
                {
                    genderinfo = "female";
                }
                else
                {
                    genderinfo = "male";
                }
                ABUUserInfoForSegment.getCurrentAndroidObject().Call("setGender", genderinfo);
                return this;
            }

            /// <summary>
            /// Sets the customInfos.
            /// </summary>
            public Builder SetCustomInfos(string customInfos)
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
                if (dictionary == null)
                {
                    return this;
                }
                AndroidJavaObject map = new AndroidJavaObject("java.util.HashMap");
                foreach (KeyValuePair<string, string> pair in dictionary)
                {
                    map.Call<string>("put", pair.Key, pair.Value);
                }
                ABUUserInfoForSegment.getCurrentAndroidObject().Call("setCustomInfos", map);
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
