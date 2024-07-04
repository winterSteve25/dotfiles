import {TrayItem} from "../types/service/systemtray";

const systemtray = await Service.import('systemtray');
const SysTrayItem = (item: TrayItem) => Widget.Button({
    child: Widget.Icon({ 
        size: 24,
    }).bind('icon', item, 'icon'),
    tooltipMarkup: item.bind('tooltip_markup'),
    onPrimaryClick: (_, event) => item.activate(event),
    onSecondaryClick: (_, event) => item.openMenu(event),
    className: "systray-item",
});

export const SysTray = () => Widget.Box({
    children: systemtray.bind('items').as(i => i.map(SysTrayItem))
});