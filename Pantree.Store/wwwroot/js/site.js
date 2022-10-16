
//------------------- Menu Toggle -------------------//

$('body').on('click', '.responsive-menu', function () {
    let toggle = $(this).attr('toggle');
    if (typeof toggle != 'undefined' && toggle !== false) {
        $('.' + toggle).toggleClass('show');
    }
});

$('body').on('click', '.navbar-menu-button', function () {
    $(this).find('.navbar-hamburger').toggleClass('open');
    $('.navbar-menu').toggle(500);
});

$(document).click(function (event) {
    var $target = $(event.target);
    if (!$target.closest('.navbar-menu-button').length && !$target.closest('.navbar-menu').length && $('.navbar-menu').is(':visible')) {
        $('.navbar-hamburger').removeClass('open');
        $('.navbar-menu').toggle(500);
    }
});

//------------------- Scroll to top -------------------//

$('body').on('click', '.btn-top', function () {
    window.scrollTo({ top: 0, behavior: 'smooth' });
});

//------------------- Dependent Dropdown -------------------//

$('body').on('change', 'select[dynamic-select-url]', function () {
    let url = $(this).attr('dynamic-select-url');
    let input = $(this).attr('dynamic-select-load');
    let param = $(this).val();

    $.ajax({
        type: 'GET',
        url: url.replace('=0', '=' + param),
        success: function (result) {
            $('#' + input).replaceWith(result);
        }
    });
});

//------------------- Update partial -------------------//

$('body').on('submit', '.load-partial', function () {
    codeReader.reset();

    let form = $(this);
    let partialContainer = form.attr('attr-partial');
    let callback = form.attr('attr-callback');

    $.ajax({
        type: form.attr('method'),
        url: form.attr('action'),
        data: new FormData(this),
        processData: false,
        contentType: false,
        success: function (result) {

            $('#' + partialContainer).empty().html(result);
            form[0].reset();

            if (callback != null) {
                window[callback]();
            }

        }
    });

    return false;
});

//------------------- Form on change -------------------//

$('body').on('change', '.submit-onchange', function () {
    let form = $(this).closest('form');
    form.submit();
});

//------------------- Number inputs -------------------//

$('body').on('input', 'input[inputmode="numeric"]', function () {
    var $input = $(this);
    $input.val($input.val().replace(/[^\d]+/g, ''));
});

$('body').on('keydown', 'input[inputmode="numeric"]', function (e) {
    let input = $(this);
    let val = input.val();

    if (e.keyCode == 38) {
        val = Number(input.val()) + Number(1);
    }
    if (e.keyCode == 40) {
        val = Number(input.val()) - Number(1);
        val = val < 0 ? 0 : val;
    }
    input.focus().val(val);

});

//------------------- Button Behaviour -------------------//

$('body').on('click', 'button[load]', function () {

    let partial = $(this).attr('partial');
    let url = $(this).attr('load');
    let callback = $(this).attr('attr-callback');

    if (typeof partial !== 'undefined' && partial !== false) {

        $.ajax({
            type: 'POST',
            url: url,
            success: function (result) {
                $('#' + partial).empty().html(result);

                if (callback != null) {
                    window[callback]();
                }
            }
        });

    } else {
        window.location = url;
    }
});

$('body').on('click', '.account-menu button, .storage-menu button', function () {
    $('.account-menu button, .storage-menu button').each(function () {
        $(this).removeClass('selected');
    });

    $(this).toggleClass('selected');
})

$('body').on('click', '.tab-tray button', function () {
    $('.tab-tray button').each(function () {
        $(this).removeClass('selected');

        var partial = $(this).attr('show');
        $('#' + partial).removeClass('active');
    });

    $(this).toggleClass('selected');
    var show = $(this).attr('show');
    $('#' + show).addClass('active');
})

//------------------- Profile Picture -------------------//

let canvas, context;

$(document).ready(function () {
    let params = new URLSearchParams(window.location.search);
    if (window.location.pathname == '/Account' && params.get('section') == 2) {
        SetupCanvas();
    }
});

function LoadProfilePicPartial() {
    $.ajax({
        type: 'POST',
        url: "/Account/ProfileImage",
        success: function (result) {
            $('#accountContainer').empty().html(result);
            SetupCanvas();
        }
    });
}

function SetupCanvas() {
    canvas = $("#Image");
    context = canvas.get(0).getContext("2d");
}

$('body').on('click', '#LoadImage_Trigger', function () {
    let input = $(this).attr('input-name');
    $('#' + input).click();
});

$('body').on('change', '#ProfilePicture_LoadImage', function () {
    if (this.files && this.files[0]) {
        if (this.files[0].type.match(/^image\//)) {

            var reader = new FileReader();
            reader.onload = function (evt) {
                var img = new Image();
                img.onload = function () {
                    context.canvas.height = img.height;
                    context.canvas.width = img.width;
                    context.drawImage(img, 0, 0);
                    var cropper = canvas.cropper({
                        aspectRatio: 1
                    });

                    $('body').on('click', '#CropImage', function () {
                        let form = $('#ProfileImageForm');
                        let data = new FormData(document.getElementById('ProfileImageForm'));

                        var croppedImageDataURL = getRoundedCanvas(canvas.cropper('getCroppedCanvas')).toDataURL("image/png", 0.5);
                        var blob = dataURItoBlob(croppedImageDataURL);
                        data.append('ProfileImage', blob);

                        $.ajax({
                            type: form.attr('method'),
                            url: form.attr('action'),
                            data: data,
                            contentType: false,
                            processData: false,
                            success: function () {
                                window.location.search = '?section=2';
                            },
                            error: function () {
                                LoadProfilePicPartial();
                            }
                        });

                        $(form).trigger("reset");
                    });
                };

                $('.upload-conceal').remove();

                img.src = evt.target.result;
            };

            reader.readAsDataURL(this.files[0]);
            $('.upload-reveal').show();
        }
    }
});

function getRoundedCanvas(sourceCanvas) {
    var canvas = document.createElement('canvas');
    var context = canvas.getContext('2d');
    var width = sourceCanvas.width;
    var height = sourceCanvas.height;

    canvas.width = width;
    canvas.height = height;
    context.imageSmoothingEnabled = true;
    context.drawImage(sourceCanvas, 0, 0, width, height);
    context.globalCompositeOperation = 'destination-in';
    context.beginPath();
    context.arc(width / 2, height / 2, Math.min(width, height) / 2, 0, 2 * Math.PI, true);
    context.fill();
    return canvas;
}

function dataURItoBlob(dataURI) {
    // convert base64/URLEncoded data component to raw binary data held in a string
    var byteString;
    if (dataURI.split(',')[0].indexOf('base64') >= 0)
        byteString = atob(dataURI.split(',')[1]);
    else
        byteString = unescape(dataURI.split(',')[1]);

    // separate out the mime component
    var mimeString = dataURI.split(',')[0].split(':')[1].split(';')[0];

    // write the bytes of the string to a typed array
    var ia = new Uint8Array(byteString.length);
    for (var i = 0; i < byteString.length; i++) {
        ia[i] = byteString.charCodeAt(i);
    }

    return new Blob([ia], {
        type: mimeString
    });
}

//------------------- Pill buttons -------------------//

(function ($) {
    $.fn.focusTextToEnd = function () {
        this.focus();
        var $thisVal = this.val();
        this.val('').val($thisVal);
        return this;
    }
}(jQuery));

$('body').on('click', '.btn-edit', function () {
    if ($(this).closest('.form-input-container').hasClass('editable-container')) {
        $(this).closest('.form-input-container').removeClass('editable-container');
        $(this).closest('.form-input-container').addClass('editing-container');
        $(this).closest('.form-input-container').find('input').focusTextToEnd();
    }    
    BtnLogic($(this));
});

$('body').on('click', '.btn-cancel', function () {
    if ($(this).closest('.form-input-container').hasClass('editing-container')) {
        $(this).closest('.form-input-container').removeClass('editing-container');
        $(this).closest('.form-input-container').addClass('editable-container');
    }
    BtnLogic($(this));
});

$('body').on('click', '.btn-save', function () {
    $(this).closest('.input').addClass('edit-submit-mode');
    BtnLogic($(this));
});

function BtnLogic(button) {

    if (button.hasClass('btn-form')) {
        button.closest('form').submit();
    }

    let reveal = button.attr('reveal');
    if (typeof reveal != 'undefined' && reveal !== false) {
        $('.' + reveal).removeClass('hidden');
    }

    let conceal = button.attr('conceal');
    if (typeof conceal != 'undefined' && conceal !== false) {
        $('.' + conceal).addClass('hidden');
    }

    let loadUrl = button.attr('loadUrl');
    let container = button.attr('container');
    if ((typeof loadUrl != 'undefined' && loadUrl !== false) && (typeof container != 'undefined' && container !== false)) {
        LoadPartial(loadUrl, container);
    }

    let callback = button.attr('callback');
    if (typeof callback != 'undefined' && callback !== false) {

        let callbackParameter = button.attr('callback-param');
        if (typeof callbackParameter != 'undefined' && callbackParameter !== false) {
            window[callback](callbackParameter);
        } else {
            window[callback]();
        }
    }
}

//----------------------- Ajax Methods -----------------//

const LoadPartial = (loadUrl, container) => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url: loadUrl,
            type: 'GET',
            success: function (result) {
                $('#' + container).empty().html(result);
                resolve();
            },
            error: function (request, error) {
                console.log(error + ". Request: " + JSON.stringify(request));
                reject(error);
            }
        });
    })
}

//----------------------- Index Form -------------------//

const codeReader = new ZXing.BrowserBarcodeReader();

$('body').on('change', '#UploadImageInput', function () {
    if (this.files && this.files[0]) {
        if (this.files[0].type.match(/^image\//)) {
            codeReader.reset();
            $(this).closest('form').find('.input-file').addClass('edit-submit-mode');
            $(this).closest('form').submit();
        }
    }
});

function LoadScanForm() {

    LoadPartial('/Scan/LoadForm', 'FormContainer').then(function() {
        $('#Code').focus();

        codeReader.decodeOnceFromVideoDevice(null, 'video').then(function (result) {
            let code = result.text;
            $('#Code').val(code);
            $('#Code').closest('form').find('.input-file').addClass('edit-submit-mode');
            $('#Code').closest('form').submit();

        }).catch((err) => { console.log(err); });
    });
}

function StopScanner() {
    codeReader.reset();
    $('#FormContainer').empty();
}

function ToggleItem(element) {
    $('#' + element).toggleClass('collapsed');
}
