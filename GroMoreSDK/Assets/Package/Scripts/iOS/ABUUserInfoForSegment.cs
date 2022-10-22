
//------------------------------------------------------------------------------
// Copyright (c) 2018-2019 Beijing Bytedance Technology Co., Ltd.
// All Right Reserved.
// Unauthorized copying of this file, via any medium is strictly prohibited.
// Proprietary and confidential.
//------------------------------------------------------------------------------

namespace ByteDance.Union
{
#if !UNITY_EDITOR && UNITY_IOS
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


        private static ABUUserInfoForSegment _segment = null;

        public static AndroidJavaObject getCurrentAndroidObject()
        {
            return null;
        }

        public static ABUUserInfoForSegment CreateSharedSegment()
        {
            if (_segment == null)
            {
                _segment = new ABUUserInfoForSegment();
            }
            return _segment;
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
                ABUUserInfoForSegment.CreateSharedSegment().userID = userID;
                return this;
            }

            /// <summary>
            /// Sets channel.
            /// </summary> 
            public Builder SetChannel(string channel)
            {
                ABUUserInfoForSegment.CreateSharedSegment().channel = channel;
                return this;
            }

            /// <summary>
            /// Sets the sub channel.
            /// </summary>
            public Builder SetSubChannel(string subChannel)
            {
                ABUUserInfoForSegment.CreateSharedSegment().subChannel = subChannel;
                return this;
            }

            /// <summary>
            /// Sets the age.
            /// </summary>
            public Builder SetAge(int age)
            {
                ABUUserInfoForSegment.CreateSharedSegment().age = age;
                return this;
            }

            /// <summary>
            /// Sets a value : userGroup
            /// </summary>
            public Builder SetUserGroup(string userGroup)
            {
                ABUUserInfoForSegment.CreateSharedSegment().userGroup = userGroup;
                return this;
            }

            /// <summary>
            /// Sets the gender.
            /// </summary>
            public Builder SetGender(ABUUserInfoGender gender)
            {
                ABUUserInfoForSegment.CreateSharedSegment().gender = gender;
                return this;
            }

             /// <summary>
            /// Sets the custom data dictionary.
            /// </summary>
            /// <returns>The custom data dictionary.</returns>
            /// <param name="dictionary">Dictionary.</param>
            public Builder setCustomDataDictionary(Dictionary<string, string> dictionary)
            {
                ABUUserInfoForSegment.CreateSharedSegment().customDataDictionary = dictionary;
                return this;
            }

            /// <summary>
            /// Build the ABUUserInfoForSegment.
            /// </summary>
            public ABUUserInfoForSegment Build()
            {
                return ABUUserInfoForSegment.CreateSharedSegment();
            }
        }
    }
#endif
}
