const $ = document.querySelector.bind(document)
const $$ = document.querySelectorAll.bind(document)


const isDropDown = $('.is-dropdown')
const dropDownMenu = $('.dropdown-menu')

//const dropDownMenuFlex = $$('.dropdown-menu-flex')
//const dropDown_isRight = $$('.is-right')

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

            if (hide_flex == false) {
                dropDownMenuFlex.classList.remove('display-block')
                dropDown_isRight.classList.remove('black-bg')
                hide_flex = true
            }
        }
        else {
            dropDownMenu.classList.remove('display-block')
            hide = true
        }
        e.stopPropagation()
    })
}
dropDM()

// dropDownMenu.addEventListener('click', (e) =>{
//     e.stopPropagation()
// })

//let hide_flex = true
//const dropDMFlex = () => {
//    dropDown_isRight.forEach(x => {
//        x.addEventListener('click', (e) => {
//            dropDownMenuFlex.forEach(y => {

//                if (hide_flex) {
//                    y.classList.add('display-block')
//                    e.classList.add('black-bg')
//                    hide_flex = false

//                    if (hide == false) {
//                        dropDownMenu.classList.remove('display-block')
//                        hide = true
//                    }
//                }
//                else {
//                    y.classList.remove('display-block')
//                    e.classList.remove('black-bg')
//                    hide_flex = true

//                }
//            })

//            e.stopPropagation()
//        })
//    })
//}
//dropDMFlex()


$('.modal').addEventListener('click', (e) => {

    if (hide == false) {
        hide = true
        dropDownMenu.classList.remove('display-block')
    }

    //if (hide_flex == false) {
    //    hide_flex = true
    //    dropDownMenuFlex.classList.remove('display-block')
    //    dropDown_isRight.classList.remove('black-bg')
    //}
})


