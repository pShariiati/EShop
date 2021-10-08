$(document).ready(function () {
    let items = document.querySelectorAll('#sliders .carousel-item');

    items.forEach((el) => {
        const minPerSlide = 4;
        let next = el.nextElementSibling
        for (var i = 1; i < minPerSlide; i++) {
            if (!next) {
                // wrap carousel by using first child
                next = items[0];
            }
            let cloneChild = next.cloneNode(true);
            el.appendChild(cloneChild.children[0]);
            next = next.nextElementSibling;
        }
    });

    // admin menus

    var counter = 1;
    $('#admin-main-menus li.list-group-item').each(function () {
        $(this).find('button').attr('id', 'dropdownMenuButton' + counter);
        $(this).find('ul').attr('aria-labelledby', 'dropdownMenuButton' + counter++);
    });

    var menus = $('#admin-main-menus').html();
    var win = $(this);
    if (win.width() > 991) {
        $('#admin-main-menus').html(menus);
        $('#admin-navbar-menus').html('');
    } else {
        $('#admin-navbar-menus').html(menus);
        $('#admin-main-menus').html('');
    }
    $(window).on('resize', function () {
        win = $(this);
        if (win.width() > 991) {
            $('#admin-main-menus').html(menus);
            $('#admin-navbar-menus').html('');
        } else {
            $('#admin-navbar-menus').html(menus);
            $('#admin-main-menus').html('');
        }
    });

    // end admin menus

});
//function showRecaptchaErrorText(selector, recaptcha) {
//    var response = grecaptcha.getResponse(recaptcha);
//    var result = response.length !== 0;
//    if (!result) {
//        $(selector + ' span[data-valmsg-for="GoogleReCaptchaResponse"]').html('لطفا کپچا را اعتبار سنجی کنید.');
//    } else {
//        $(selector + ' span[data-valmsg-for="GoogleReCaptchaResponse"]').html('');
//    }
//}

//$('#login-form button[type="submit"]').click(function () {
//    showRecaptchaErrorText('#login-form', loginNavbarRecaptcha);
//});

//$('#register-form button[type="submit"]').click(function () {
//    showRecaptchaErrorText('#register-form', registerNavbarRecaptcha);
//});

function onBeginForLogin() {
    //var response = grecaptcha.getResponse();
    //var result = response.length !== 0;
    //if (!result) {
    //    $('#login-form span[data-valmsg-for="GoogleReCaptchaResponse"]').html('لطفا کپچا را اعتبار سنجی کنید.');
    //} else {
    //    $('#login-form button[type="submit"]').attr('disabled', 'disabled');
    //    $('#login-form button[type="submit"]').parent().find('p').addClass('d-none');
    //    $('#login-errors-place ul').html('');
    //}
    $('#login-form button[type="submit"]').attr('disabled', 'disabled');
    $('#login-form button[type="submit"]').parent().find('p').addClass('d-none');
    $('#login-errors-place ul').html('');
    //return result;
}

function onCompleteForLogin() {
    $('#login-form button[type="submit"]').removeAttr('disabled');
}

function onSuccessForLogin(data, status, xhr) {
    if (data == 'Success') {
        $('#login-form input').val('');
        $('#login-form p').removeClass('d-none');
        $('#login-form div').remove();
        var currentPath = window.location.pathname;
        if (currentPath == '/Account/ConfirmationAccount') {
            location.href = '/';
        } else {
            location.reload();
        }
    }
}

function onFailureForLogin(xhr) {
    putErrorsInElement("#login-errors-place", xhr.responseJSON);
    if (xhr.responseJSON[0] == 'شما قبلا وارد سیستم شده اید')
        location.reload();
}

//////////////////////////////

function onBeginForRegister() {
    $('#register-form button[type="submit"]').attr('disabled', 'disabled');
    $('#register-form p').addClass('d-none');
    $('#register-errors-place ul').html('');
}

function onCompleteForRegister() {
    $('#register-form button[type="submit"]').removeAttr('disabled');
}
function onSuccessForRegister(data, status, xhr) {
    if (data == 'Success') {
        $('#register-form input').val('');
        $('#register-form p').removeClass('d-none');
        $('#register-form div').remove();
    }
}
function onFailureForRegister(xhr) {
    putErrorsInElement("#register-errors-place", xhr.responseJSON);
    if (xhr.responseJSON[0] == 'شما قبلا وارد سیستم شده اید')
        location.reload();
}

function putErrorsInElement(element, errors) {
    $(element).html('<ul></ul>');
    errors.forEach(error => {
        $(element + ' ul').append('<li>' + error + '</li>');
    });
}

//var loginNavbarRecaptcha;
//var registerNavbarRecaptcha;
//var loginPageRecaptcha;
//function onloadCaptchaCallback() {
//    loginNavbarRecaptcha = grecaptcha.render('login-nabar-recaptcha');
//    registerNavbarRecaptcha = grecaptcha.render('register-nabar-recaptcha');
//    if ($('#login-page-recaptcha').length) {
//        loginPageRecaptcha = grecaptcha.render('login-page-recaptcha');
//    }
//    $('.captcha-loading').remove();
//}

//$('#login-page-form').submit(function () {
//    var response = grecaptcha.getResponse();
//    var result = response.length !== 0;
//    if (!result) {
//        $('#login-page-form span[data-valmsg-for="GoogleReCaptchaResponse"]').html('لطفا کپچا را اعتبار سنجی کنید.');
//    }
//    return result;
//});

//function loginPageRecaptchaCallBack() {
//    removeRecaptchaErrorText('#login-page-form');
//}

//function loginNavbarRecaptchaCallBack() {
//    removeRecaptchaErrorText('#login-form');
//}

//function registerNavbarRecaptchaCallBack() {
//    removeRecaptchaErrorText('#register-form');
//}

//function removeRecaptchaErrorText(selector) {
//    $(selector + ' span[data-valmsg-for="GoogleReCaptchaResponse"]').html('');
//}

var myModalEl = document.getElementById('login-register-modal');
if (myModalEl) {
    myModalEl.addEventListener('shown.bs.modal', function (event) {
        $.get('/Account/LoadLoginPartial',
            function (data, status) {
                $('#login-tab-place').html(data);
                $.validator.unobtrusive.parse($('#login-form'));
                var currentUrlPathname = window.location.pathname;
                $('form#external-login-form').attr('action', '/Account/ExternalLogin?returnUrl=' + currentUrlPathname);
            });
    });
}

$('#register-tab').click(function () {
    $.get('/Account/LoadRegisterPartial',
        function (data, status) {
            $('#register-tab-place').html(data);
            $.validator.unobtrusive.parse($('#register-form'));
        });
});

if (typeof isUserAuthenticated != 'undefined' && isUserAuthenticated) {
    reloadTotalPriceInNavbar();
}

function reloadTotalPriceInNavbar() {
    $('#checkout-button span').html('لطفا صبر کنید ...');
    $.get('/Cart/GetUserCartTotalPrice',
        function (data) {
            $('#checkout-button span').html(data);
        });
}

var myOffcanvas = document.getElementById('offcanvasScrolling');
if (myOffcanvas != null) {
    myOffcanvas.addEventListener('show.bs.offcanvas',
        function () {
            getCardDetails();
        });
}
function getCardDetails() {
    if (isUserAuthenticated) {
        $('#offcanvasScrolling .offcanvas-body').html('');
        $('#cart-details-loading').removeClass('d-none');
        $.get('/Cart/ShowCartDetailsPreview',
            function (result) {
                $('#cart-details-loading').addClass('d-none');
                $('#offcanvasScrolling .offcanvas-body').html(result);
            });
    }
}

function increaseOrLowOffCartDetail(productId, isIncrease, el, removeAll) {
    // غیر فعال کردن دکمه های مثبت منفی
    $(el).closest('.d-block').find('div:eq(0) button, div:eq(2) button, button[class*="text-danger"]')
        .attr('disabled', 'disabled');
    $.get('/Cart/IncreaseOrLowOff',
        { productId: productId, isIncrease: isIncrease, removeAll: removeAll },
        function () {
            // فعال سازی مجدد دکمه های مثبت منفی
            $(el).closest('.d-block').find('div:eq(0) button, div:eq(2) button, button[class*="text-danger"]')
                .removeAttr('disabled');
            if (false) {
                reloadTotalPriceInNavbar();
                getCardDetails();
            }
            else {
                // گرفتن 2 دیو بالاتر از خودش
                var selector = $(el).closest('.d-block');
                // تعداد این محصول اضافه شده به سبد خرید
                var countOfCartDetail = parseInt(
                    $(selector).find('div:eq(1) input').val()
                );
                // قیمت واحد این محصول را میگریم
                // قیمت واحد دارای جدا کننده است آن
                // جدا کننده ها را با استفاده از دستور زیر پاک میکنیم
                var uniquePriceOfCartDetail = parseInt(
                    $(selector).find('p:first span').html().replace(/,/g, '')
                );
                // اگر تعداد این محصول اضافه شده یک بود و کاربر روی دکمه
                // منفی کلیک کرد باید این دیو را کلا پاک کنیم
                if (countOfCartDetail === 1 && !isIncrease) {
                    $(el).closest('.card').remove();
                }
                else if (removeAll) {
                    $(el).closest('.card').remove();
                }
                // در غیر کاربر یا میخواهد تعداد را زیاد کند یا کم کند
                // اما مطمئنیم که تعداد محصولات 1 نیست
                else {
                    // تعداد جدید
                    // اگر روی مثبت کلیک کرده باشد اضافه میکنیم در غیر
                    // اینصورت کم میکنیم.
                    var newCount = isIncrease ? countOfCartDetail + 1 : countOfCartDetail - 1;
                    // مقدار جدید را در اینپوت مربوطه نمایش میدهیم
                    $(selector).find('div:eq(1) input').val(newCount);
                    // قیمت جدید را محاسبه میکنیم
                    // قیمت واحد ضربدر تعداد
                    var newPrice = uniquePriceOfCartDetail * newCount;
                    // و بعد آنرا در مکان مربوطه نمایش میدهیم
                    // آنرا سه رقم سه رقم جدا میکنیم
                    $(selector).find('p:eq(1) span').html(
                        newPrice.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ',')
                    );
                }
                // قیمت کل را میگیریم
                // جدا کننده های قیمت کل را گرفته و آن را
                // به یک عدد صیحیح تبدیل میکنیم
                var cartTotalPrice = parseInt(
                    $('#cart-total-price').html().replace(/,/g, '')
                );
                // قیمت کل جدید
                var newCartTotalPrice;
                // اگر دکمه مثبت کلیک شده باشد قیمت جدید
                // را به علاوه قیمت کل فعلی میکنیم
                // در نتیجه قیمت جدید محاسبه میشود
                if (isIncrease) {
                    newCartTotalPrice = (cartTotalPrice + uniquePriceOfCartDetail).toString();
                }
                // اگر دکمه مفی کلیک شده باشد قیمت جدید
                // را منهای قیمت کل فعلی میکنیم
                // در نتیجه قیمت جدید محاسبه میشود
                else if (removeAll) {
                    newCartTotalPrice = (cartTotalPrice - (uniquePriceOfCartDetail * countOfCartDetail)).toString();
                }
                else {
                    newCartTotalPrice = (cartTotalPrice - uniquePriceOfCartDetail).toString();
                }
                // قیمت کل جدید را در جای خودش نمایش میدهیم
                // قیمت کل جدید را سه رقم سه رقم جدا میکنیم
                $('#cart-total-price, #checkout-button span').html(
                    newCartTotalPrice.replace(/\B(?=(\d{3})+(?!\d))/g, ',')
                );
            }
        });
}