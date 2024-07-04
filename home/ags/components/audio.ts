const audio = await Service.import('audio')

export const VolumeIndicator = () => Widget.EventBox({
    child: Widget.CircularProgress({
        child: Widget.Icon({
            size: 24,
            css: "color: @theme_text_color",
        }).hook(audio.speaker, self => {
            const vol = audio.speaker.volume * 100;
            const icon = [
                [100, 'high'],
                [47, 'medium'],
                [1, 'low'],
                [0, 'muted'],
            ].find(([threshold]) => threshold <= vol)?.[1];

            self.icon = `audio-volume-${icon}-symbolic`;
            self.tooltip_text = `${audio.speaker.name}: ${Math.floor(vol)}%`;
        }),
        value: audio.speaker.bind("volume").as(vol => vol > 0 ? vol : 0),
    }),
    onPrimaryClick: () => audio.speaker.is_muted = !audio.speaker.is_muted,
    onScrollUp: () => audio.speaker.volume = Math.min(audio.speaker.volume + 0.05, 1),
    onScrollDown: () => audio.speaker.volume = Math.max(audio.speaker.volume - 0.05, 0),
    classNames: [ "volume" ],
    marginTop: 8,
    marginBottom: 8,
})