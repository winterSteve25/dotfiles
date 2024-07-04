const bluetooth = await Service.import('bluetooth')

const connectedList = Widget.Box({
    setup: self => self.hook(bluetooth, self => {
        self.children = bluetooth.connected_devices
            .map(({ icon_name, name }) => Widget.Box([
                Widget.Icon(icon_name + '-symbolic'),
                Widget.Label(name),
            ]));

        self.visible = bluetooth.connected_devices.length > 0;
    }, 'notify::connected-devices'),
})

export const BluetoothIndicator = () => Widget.ToggleButton({
    child: Widget.Icon({
        icon: bluetooth.bind('enabled').as(on =>
            `bluetooth-${on ? 'active' : 'disabled'}-symbolic`),
    }),
    onToggled: ({ active }) => bluetooth.enabled = active,
    classNames: ["bghaver"],
})