const network = await Service.import('network')

const WifiIndicator = () => Widget.Icon({
    tooltipMarkup: network.wifi.bind('ssid').as(ssid => `Connected to: ${ssid}` || 'Unknown'),
    icon: network.wifi.bind('icon_name'),
    size: 16,
    className: "bghaver",
})

const WiredIndicator = () => Widget.Icon({
    icon: network.wired.bind('icon_name'),
    tooltipMarkup: network.wired.bind("internet"),
    size: 16,
    className: "bghaver"
})

export const NetworkIndicator = () => Widget.Stack({
    children: {
        wifi: WifiIndicator(),
        wired: WiredIndicator(),
    },
    shown: network.bind('primary').as(p => p || 'wifi'),
})