const Bar = (monitor: number) => Widget.Window({
    name: `bar-${monitor}`,
    anchor: [ "top", "left", "right" ],
    child: Widget.Label('hello'),
    class_name: "bar",
});

App.config({
    windows: [Bar(0)],
    style: "./style.css",
});

Utils.monitorFile(
    `${App.configDir}/style`,
    () => {
        console.log("Switching css");
        const css = `${App.configDir}/style.css`;
        App.resetCss();
        App.applyCss(css);
    }
);