using System;
using System.Collections.Generic;

namespace ByteDance.Union
{
    // 目录的类型
    public enum ItemTypeEnum : int
    {
        Text = 1, // 文本类型
        Input = 2, // 输入类型
    }

    // 列表的基本数据
    [Serializable]
    public class BaseListData
    {
        // 条目Id，从1开始自增长
        public int Id;

        // 名称id，方便看出是哪个条目，在整个列表中id唯一（包括1级，2级、3级、4级列表），点击事件可以按照这个Id来区分
        public string NameId;

        // 中文描述内容
        public string CnContent;

        // 英文描述内容
        public string ZnContent;

        // 获取要显示的内容
        public string GetContent()
        {
            // 检测当前设置的语言是否是中文
            return LiteDataUtil.GetLanguageType() == LanguageType.Chinese ? this.CnContent : this.ZnContent;
        }


        // Item是否是测试功能，true-是测试功能，false-非测试功能,默认为非测试功能
        public bool TestFunction = false;

        // 是否是测试功能
        public bool IsTestFunction()
        {
            return TestFunction;
        }

        // 平台功能，0-通用，1-Android，2-ios
        public int Platform = 0;

        // 是否通用功能
        public bool IsCommonFunction()
        {
            return Platform == 0;
        }

        // 是否是Android功能
        public bool IsAndroidFunction()
        {
            return Platform == 1;
        }

        // 是否是ios功能
        public bool IsIOSFunction()
        {
            return Platform == 2;
        }

    }

    // 主页面列表实体类
    [Serializable]
    public class MainListEntity
    {
        // 一级列表集合
        public List<PrimaryLevelFunctionsEntity> PrimaryLevelFunctionsEntities;
    }

    // 主页面一级列表实体类
    [Serializable]
    public class PrimaryLevelFunctionsEntity : BaseListData
    {
        // 二级列表集合
        public List<SecondLevelFunctionsEntity> SecondLevelFunctionsEntities;
    }

    // 主页面二级列表实体类
    [Serializable]
    public class SecondLevelFunctionsEntity : BaseListData
    {
        // 列表是否展开，！！！ 此字段不需要配置在json中，默认是不展开
        public bool IsExtend;

        // 三级列表集合
        public List<ThirdLevelFunctionsEntity> ThirdLevelFunctionsEntities;
    }

    // 主页面三级列表实体类
    [Serializable]
    public class ThirdLevelFunctionsEntity : BaseListData
    {
        // 四级列表
        public List<FourLevelFunctionsEntity> FourLevelFunctionsEntities;

        // Item类型，1-文本类型，2-输入类型,默认为文本类型
        //public int ItemType = 1;

        //// 获取item的类型
        //public ItemTypeEnum GetItemType()
        //{
        //    return this.ItemType == 1 ? ItemTypeEnum.Text : ItemTypeEnum.Input;
        //}

        // 检查有没有四级列表
        public bool HasFourFunctions()
        {
            return this.FourLevelFunctionsEntities != null && this.FourLevelFunctionsEntities.Count != 0;
        }
    }

    // 实际上4级列表就是详情页面了
    [Serializable]
    public class FourLevelFunctionsEntity : BaseListData
    {
    }
}