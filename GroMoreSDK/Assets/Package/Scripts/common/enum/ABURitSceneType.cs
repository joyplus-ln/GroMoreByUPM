namespace ByteDance.Union
{
    /// <summary>
    /// 广告scene类型
    /// Login type.
    /// </summary>
    public enum ABURitSceneType
    {
        BURitSceneType_custom                  = 0,//custom
        ABURitSceneType_home_open_bonus         = 1,//Login/open rewards (login, sign-in, offline rewards doubling, etc.)
        ABURitSceneType_home_svip_bonus         = 2,//Special privileges (VIP privileges, daily rewards, etc.)
        ABURitSceneType_home_get_props          = 3,//Watch rewarded video ad to gain skin, props, levels, skills, etc
        ABURitSceneType_home_try_props          = 4,//Watch rewarded video ad to try out skins, props, levels, skills, etc
        ABURitSceneType_home_get_bonus          = 5,//Watch rewarded video ad to get gold COINS, diamonds, etc
        ABURitSceneType_home_gift_bonus         = 6,//Sweepstakes, turntables, gift boxes, etc
        ABURitSceneType_game_start_bonus        = 7,//Before the opening to obtain physical strength, opening to strengthen, opening buff, task props
        ABURitSceneType_game_reduce_waiting     = 8,//Reduce wait and cooldown on skill CD, building CD, quest CD, etc
        ABURitSceneType_game_more_opportunities = 9,//More chances (resurrect death, extra game time, decrypt tips, etc.)
        ABURitSceneType_game_finish_rewards     = 10,//Settlement multiple times/extra bonus (completion of chapter, victory over boss, first place, etc.)
        ABURitSceneType_game_gift_bonus         = 11//The game dropped treasure box, treasures and so on
    }
}
