using System.Collections.Generic;
using UnityEngine;

namespace ByteDance.Union
{
    public class MainListDataManager : Singeton<MainListDataManager>
    {
        // 主列表json文件路径，不要带后缀喔！！！
        private const string MainViewJsonPath = "Json/mainList";

        // 首页列表数据内容
        private MainListEntity _mainListEntity;

        // 获取首页列表数据
        public MainListEntity GetMainListEntity()
        {

            return this._mainListEntity ?? this.LoadMainListData();
        }


        // 加载首页列表数据
        private MainListEntity LoadMainListData()
        {
            var textAsset = Resources.Load<TextAsset>(MainViewJsonPath);
            return this.filterMainListData(JsonUtility.FromJson<MainListEntity>(textAsset.text));
        }

        // 过滤菜单菜单数据
        private MainListEntity filterMainListData(MainListEntity entity)
        {
            if (entity != null)
            {
                entity.PrimaryLevelFunctionsEntities = this.FilterPrimaryLevel(entity.PrimaryLevelFunctionsEntities);
            }

            return entity;
        }

        // 过滤一级菜单
        private List<PrimaryLevelFunctionsEntity> FilterPrimaryLevel(List<PrimaryLevelFunctionsEntity> primary)
        {
            if (primary != null && primary.Count > 0)
            {
                for (var i = 0; i < primary.Count; i++)
                {
                    if (primary[i] != null)
                    {
                        primary[i].SecondLevelFunctionsEntities =
                            this.FilterSecondLevel(primary[i].SecondLevelFunctionsEntities);
                    }

                    // 如果需要删除一级菜单：primary.Remove(primary[i])
                }
            }

            return primary;
        }

        // 过滤二级菜单
        private List<SecondLevelFunctionsEntity> FilterSecondLevel(List<SecondLevelFunctionsEntity> second)
        {
            if (second != null && second.Count > 0)
            {
                for (int i = 0; i < second.Count; i++)
                {
                    var item = second[i];
                    if (item != null)
                    {
                        item.ThirdLevelFunctionsEntities =
                            this.FilterThirdLevel(item.ThirdLevelFunctionsEntities);

                        // 如果需要删除二级菜单：second.Remove(second[i])

                        // 如果是Android端功能
                        if (item.IsAndroidFunction())
                        {
#if !UNITY_ANDROID
                            second.Remove(item);
                            item = null;
#endif
                        }
                        else if (item.IsIOSFunction())
                        {
                            // 如果是iOS端功能
#if !UNITY_IOS
                            second.Remove(item);
                            item = null;
#endif
                        }
                    }
                }
            }

            return second;
        }

        // 过滤三级菜单
        private List<ThirdLevelFunctionsEntity> FilterThirdLevel(List<ThirdLevelFunctionsEntity> third)
        {
            if (third != null && third.Count > 0)
            {
                for (var i = third.Count - 1; i >= 0; i--)
                {
                    var item = third[i];

                    // 如果不为空
                    if (item != null)
                    {
                        // 是测试功能 && 不是测试环境
                        if (item.IsTestFunction() && !GlobalUtil.IsDebug)
                        {
                            third.Remove(item);
                            item = null;
                        }
                        else
                        {
                            // 如果是Android端功能
                            if (item.IsAndroidFunction())
                            {
#if !UNITY_ANDROID
                                third.Remove(item);
                                item = null;
#endif
                            }
                            else if (item.IsIOSFunction())
                            {
                                // 如果是iOS端功能
#if !UNITY_IOS
                                third.Remove(item);
                                item = null;
#endif
                            }
                        }
                    }

                    if (item != null)
                    {
                        item.FourLevelFunctionsEntities = this.FilterFourLevel(item.FourLevelFunctionsEntities);
                    }
                }
            }

            return third;
        }

        // 过滤四级菜单
        private List<FourLevelFunctionsEntity> FilterFourLevel(List<FourLevelFunctionsEntity> four)
        {
            return four;
        }
    }
}