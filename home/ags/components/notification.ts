const notifications = await Service.import("notifications")

function NotificationIcon({app_entry, app_icon, image}) {
    if (image) {
        return Widget.Box({
            css: `background-image: url("${image}");`
                + "background-size: contain;"
                + "background-repeat: no-repeat;"
                + "background-position: center;",
        })
    }

    let icon = "dialog-information-symbolic"
    if (Utils.lookUpIcon(app_icon))
        icon = app_icon

    if (app_entry && Utils.lookUpIcon(app_entry))
        icon = app_entry

    return Widget.Box({
        child: Widget.Icon(icon),
    })
}

function Notification(n) {
	Utils.timeout(8000, () => {
        self.class_names = ["invis", ...self.class_names];
        Utils.timeout(200, n.dismiss);
	});

    const icon = Widget.Box({
        vpack: "start",
        class_name: "icon",
        child: NotificationIcon(n),
    })

    const title = Widget.Label({
        class_name: "title",
        xalign: 0,
        justification: "left",
        hexpand: true,
        max_width_chars: 24,
        truncate: "end",
        wrap: true,
        label: n.summary,
        use_markup: true,
    })

    const body = Widget.Label({
        class_name: "body",
        hexpand: true,
        use_markup: true,
        xalign: 0,
        justification: "left",
        label: n.body,
        wrap: true,
    })

    const actions = Widget.Box({
        class_name: "actions",
        children: n.actions.map(({id, label}) => Widget.Button({
            class_name: "action-button",
            on_clicked: () => {
                n.invoke(id)
                n.dismiss()
            },
            hexpand: true,
            child: Widget.Label(label),
        })),
    })

    return Widget.EventBox({
        attribute: {id: n.id},
        className: "notif-box",
        onPrimaryClickRelease: self => {
            self.class_names = ["invis", ...self.class_names];
            Utils.timeout(200, n.dismiss);
        },
        child: Widget.Box({
            class_name: `notification ${n.urgency}`,
            vertical: true,
            children: [
                Widget.Box([
                    icon,
                    Widget.Box(
                        {vertical: true},
                        title,
                        body,
                    ),
                ]),
                actions,
            ]
        })
    })
}

export function NotificationPopups(monitor = 0) {
    const list = Widget.Box({
        vertical: true,
        children: notifications.popups.map(Notification),
    })

    const notifs = {};

    function onNotified(_, id: number) {
        const n = notifications.getNotification(id)
        if (!n) return;
        const widget = Notification(n);
        list.children = [widget, ...list.children]
        notifs[id] = widget;
    }

    function onDismissed(_, id: number) {
        let self = notifs[id];
        if (!self) return;
        self.class_names = ["invis", ...self.class_names];
        Utils.timeout(200, () => self.destroy());
        notifs[id] = null;
    }

    list.hook(notifications, onNotified, "notified")
        .hook(notifications, onDismissed, "dismissed")

    return Widget.Window({
        monitor: monitor,
        name: `notifications${monitor}`,
        class_name: "notification-popups",
        anchor: ["top", "right"],
        child: Widget.Box({
            css: "min-width: 2px; min-height: 2px;",
            class_name: "notifications",
            vertical: true,
            child: list,
        }),
    })
}
