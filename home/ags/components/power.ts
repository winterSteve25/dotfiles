const battery = await Service.import('battery')

export const BatteryIndicator = () => Widget.CircularProgress({
    child: Widget.Icon({
		setup: self => {
			self.hook(battery, () => {
				let charging = battery.charging;
            	const icon = [
					[100, 'full'],
					[80, 'good'],
					[60, 'medium'],
					[40, 'low'],
					[20, 'caution'],
				].find(([threshold]) => (threshold as number) <= battery.percent)?.[1];

				if (charging) {
					self.icon = `battery-${icon}-charging-symbolic`;
				} else {
					self.icon = `battery-${icon}-symbolic`;
				}

            	self.tooltip_text = `Fully charged in ${Math.floor(battery.time_remaining / 60)}m`;
			})
		},
		css: "color: @theme_text_color;",
        size: 24,
    }),
    visible: battery.bind("available"),
    value: battery.bind('percent').as(p => p > 0 ? p / 100 : 0),
    classNames: battery.bind('charging').as(ch => ch ? [ 'battery-charging', "bghaver" ] : [ 'battery', 'bghaver' ]),
})
