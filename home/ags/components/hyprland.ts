const hyprland = await Service.import('hyprland')

export const FocusedTitle = () => Widget.Label({
    label: hyprland.active.client.bind('title'),
    visible: hyprland.active.client.bind('address')
        .as(addr => !!addr),
})