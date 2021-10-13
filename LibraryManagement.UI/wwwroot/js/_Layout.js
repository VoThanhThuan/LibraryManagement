const $ = document.querySelector.bind(document)
const $$ = document.querySelectorAll.bind(document)


const isDropDown = $('.is-dropdown')
const dropDownMenu = $('.dropdown-menu')

const listTab = $$('.tab-icon')

listTab.forEach(tab => {
    tab.onclick = function () {
        $('.active').classList.remove('active')
        this.classList.add('active')
    }
})

let hide = true
const dropDM = () => {
    isDropDown.addEventListener("click", (e) => {
        if (hide) {
            dropDownMenu.classList.add('display-block')
            hide = false
        }
        else {
            dropDownMenu.classList.remove('display-block')
            hide = true
        }
        e.stopPropagation()
    })
}
dropDM()

dropDownMenu.addEventListener("click", (e) => {
    e.stopPropagation()
})

$('.modal').addEventListener('click', (e) => {

    if (hide == false) {
        hide = true
        dropDownMenu.classList.remove('display-block')
    }
})

//Form create, edit, detels, delete

const date = $('.date-of-birth')
function checkValue(str, max) {
    if (str.charAt(0) !== '0' || str == '00') {
        let num = parseInt(str);
        if (isNaN(num) || num <= 0 || num > max) num = 1
        str = num > parseInt(max.toString().charAt(0))
            && num.toString().length == 1 ? '0' + num : num.toString()
    };
    return str
};
date.addEventListener('input', function (e) {
    this.type = 'text'
    let input = this.value
    if (/\D\/$/.test(input)) input = input.substr(0, input.length - 3)
    let values = input.split('/').map(function (v) {
        return v.replace(/\D/g, '')
    });
    if (values[0]) values[0] = checkValue(values[0], 12)
    if (values[1]) values[1] = checkValue(values[1], 31)
    let output = values.map(function (v, i) {
        return v.length == 2 && i < 2 ? v + ' / ' : v
    });
    this.value = output.join('').substr(0, 14);
});

//window.onscroll = () => {
//    const scroll = $('.form-header')
//    this.scrollY > 90 ? scroll.classList.add('is-stuck') :
//        scroll.classList.remove('is-stuck')
//}

