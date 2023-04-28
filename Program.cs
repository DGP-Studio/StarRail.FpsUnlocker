using Microsoft.Win32;
using System;
using System.CommandLine;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;

namespace StarRail.FpsUnlocker;

internal static class Program
{
    private const string GraphicsSettingsModelKeyName = "GraphicsSettings_Model_h2986158309";

    public static void Main(string[] args)
    {
        RootCommand rootCommand = new("崩坏：星穹铁道 帧率解锁");
        Option<RegionType> regionOption = new("--region", "游玩的服务器 (默认 CN 国服)");
        rootCommand.AddOption(regionOption);

        rootCommand.SetHandler(SetRegistryValueFps, regionOption);

        rootCommand.Invoke(args);
    }

    private static void SetRegistryValueFps(RegionType region)
    {
        string subKeyPath = region switch
        {
            RegionType.CN => @"Software\miHoYo\崩坏：星穹铁道",
            RegionType.OS => @"Software\Cognosphere\Star Rail",
            _ => string.Empty,
        };

        if(string.IsNullOrEmpty(subKeyPath) )
        {
            ReadLine("请指定 --region 参数");
            return;
        }

        if (Registry.CurrentUser.OpenSubKey(subKeyPath, true) is RegistryKey node)
        {
            if (node.GetValue(GraphicsSettingsModelKeyName) is byte[] rawBytes)
            {
                GraphicsSettings graphicsSettings = JsonSerializer.Deserialize(rawBytes, GraphicsSettingsContext.Default.GraphicsSettings)!;
                graphicsSettings.Fps = 120;

                string result = JsonSerializer.Serialize(graphicsSettings, GraphicsSettingsContext.Default.GraphicsSettings);
                node.SetValue(GraphicsSettingsModelKeyName, Encoding.UTF8.GetBytes(result), RegistryValueKind.Binary);
                ReadLine("设置完成");
            }
            else
            {
                ReadLine("获取注册表值失败，请尝试在游戏中修改一次图形设置并关闭设置界面");
            }
        }
        else
        {
            ReadLine("打开注册表键失败，请尝试在游戏中修改一次图形设置并关闭设置界面");
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void ReadLine(string hint)
    {
        Console.WriteLine(hint);
        _ = Console.ReadLine();
    }
}