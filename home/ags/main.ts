import {TimeLabel} from "./components/time";
import {SysTray} from "./components/systray";
import {NetworkIndicator} from "./components/wifi";
import {BatteryIndicator} from "./components/power";
import {VolumeIndicator} from "./components/audio";
import {FocusedTitle} from "./components/hyprland";
import {NotificationPopups} from "./components/notification";

async function Notify() {
    console.log("Not");
    await Utils.notify({
        summary: "Notification Popup Example",
        iconName: "info-symbolic",
        body: "Lorem ipsum dolor sit amet, qui minim labore adipisicing "
            + "minim sint cillum sint consectetur cupidatat.",
        actions: {
            "Cool": () => print("pressed Cool"),
        },
    });
}

const End = () => Widget.Box({
    vertical: false,
    hpack: "end",
    className: "end",
    spacing: 8,
    children: [
        // BluetoothIndicator(),
        VolumeIndicator(),
        BatteryIndicator(),
        TimeLabel(),
        NetworkIndicator(),
    ]
});

const Bar = (monitor: number) => Widget.Window({
	monitor: monitor,
    name: `bar-${monitor}`,
    anchor: [ "top", "left", "right" ],
    exclusivity: "exclusive",
    layer: "top",
    className: "bar",
    child: Widget.CenterBox({
        startWidget: SysTray(),
        centerWidget: FocusedTitle(),
        endWidget: End(),
    }),
});

App.config({
    windows: [
        Bar(0),
        NotificationPopups(0),
    ],
    style: "./style.css",
});

Utils.monitorFile(
    `${App.configDir}/style.css`,
    () => {
        const css = `${App.configDir}/style.css`;
        App.resetCss();
        App.applyCss(css);
    }
);
