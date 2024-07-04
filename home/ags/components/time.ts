const time = Variable('', {
    poll: [10000, getTime]
})

function getTime() {
    let date = new Date();
    let min = date.getMinutes();
    return `${date.getHours()}:${min < 10 ? `0${min}` : min}`;
}

function getDate() {
    let date = new Date();
    return `Today is ${date.getMonth() + 1}/${date.getDate()}/${date.getFullYear()}`
}

export const TimeLabel = () => Widget.Label({
    classNames: ["text", "bghaver"],
    tooltipMarkup: getDate(),
    label: time.bind(),
});