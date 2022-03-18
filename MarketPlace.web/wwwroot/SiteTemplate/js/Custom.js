 
function open_waiting(selector = 'body') {
    $(selector).waitMe({
        effect: 'facebook',
        text: 'لطفا صبر کنید ...',
        bg: 'rgba(255,255,255,0.7)',
        color: '#000'
    });
}

function close_waiting(selector = 'body') {
    $(selector).waitMe('hide');
}

function ShowMessage(title, text, theme) {
    window.createNotification({
        closeOnClick: true,
        displayCloseButton: false,
        positionClass: 'nfc-bottom-right',
        showDuration: 4000,
        theme: theme !== '' ? theme : 'success'
    })({
        title: title !== '' ? title : 'اعلان',
        message: decodeURI(text)
    });
}

$(function () {
    var editors = $("[ckEditor]")
    if (editors.length > 0) {
        $.getScript('/SiteTemplate/js/ckeditor.js', (script, textStatus, jqXHR) => {
            $(editors).each((index, val) => {
                const id = $(val).attr('ckEditor');

                ClassicEditor.create(document.querySelector('[ckeditor="' + id + '"]'),
                    {
                        toolbar: {
                            items: [
                                'heading',
                                '|',
                                'bold',
                                'italic',
                                'link',
                                '|',
                                'fontSize',
                                'fontColor',
                                '|',
                                'imageUpload',
                                'blockQuote',
                                'insertTable',
                                'undo',
                                'redo',
                                'codeBlock'
                            ]
                        },
                        language: 'fa',
                        table: {
                            contentToolbar: [
                                'tableColumn',
                                'tableRow',
                                'mergeTableCells'
                            ]
                        },
                        licenseKey: '',
                        simpleUpload: {
                            // The URL that the images are uploaded to.
                            uploadUrl: '/Uploader/UploadImage'
                        }

                    })
                    .then(editor => {
                        window.editor = editor;
                    }).catch(err => {
                        console.error(err);
                    });
    
            })
        })
    }
});
 

(function () {
    var editors = $("[ckEditor]");
    if (editors.length > 0) {
        $.getScript('/SiteTemplate/js/ckeditor.js', function () {
            $(editors).each(function (index, value) {
                var id = $(value).attr('ckEditor');
                ClassicEditor.create(document.querySelector('[ckEditor="' + id + '"]'),
                    {
                        toolbar: {
                            items: [
                                'heading',
                                '|',
                                'bold',
                                'italic',
                                'link',
                                '|',
                                'fontSize',
                                'fontColor',
                                '|',
                                'imageUpload',
                                'blockQuote',
                                'insertTable',
                                'undo',
                                'redo',
                                'codeBlock'
                            ]
                        },
                        language: 'fa',
                        table: {
                            contentToolbar: [
                                'tableColumn',
                                'tableRow',
                                'mergeTableCells'
                            ]
                        },
                        licenseKey: '',
                        simpleUpload: {
                            // The URL that the images are uploaded to.
                            uploadUrl: '/Uploader/UploadImage'
                        }

                    })
                    .then(editor => {
                        window.editor = editor;
                    }).catch(err => {
                        console.error(err);
                    });
            });
        });
    }
});

function FillPageId(pageId) {
    $("#PageId").val(pageId);
    $("#filter-form").submit();
}

function isEmptyOrSpaces(str) {
    if (str == null || str.trim() === '') {
        return true;
    }
    return false;
    //return str === null || str.match(/^ *$/) === null;
}

$("#AddProductColorToList").click(function (e) {
    e.preventDefault();
    var colorName = $('#product-color-name-inp').val();
    var colorPrice = $('#product-color-price-inp').val();
    var colorCode = $('#product-color-code-inp').val();
    var currentColorsCount = $('#list-of-product-colors tr').length;
    console.log(currentColorsCount)
    if (isEmptyOrSpaces(colorName)) { return ShowMessage('اخطار', 'نام رنگ را وارد کنید', 'warning') };
    if (colorPrice == "" || colorPrice == null) { return ShowMessage('اخطار', 'قیمت رنگ را وارد کنید', 'warning') };
    if (isEmptyOrSpaces(colorCode)) { return ShowMessage('اخطار', 'کد رنگ را وارد کنید', 'warning') }
    $('#product-color-name-inp').val(' ');
    $('#product-color-price-inp').val(' ');
    var colorNameNode = `<input type="hidden" value="${colorName}" name="ProductColors[${currentColorsCount}].ColorName" color-name-hidden-inp="${currentColorsCount}"/>`;
    var colorPriceNode = `<input type="hidden" value="${colorPrice}" name="ProductColors[${currentColorsCount}].Price" color-price-hidden-inp="${currentColorsCount}"/>`;
    var colorCodeNode = `<input type="hidden" value="${colorCode}" name="ProductColors[${currentColorsCount}].ColorCode" color-code-hidden-inp="${currentColorsCount}"/>`;
    $('#create-product-form').append(colorNameNode);
    $('#create-product-form').append(colorPriceNode);
    $('#create-product-form').append(colorCodeNode);
    var tableNode = `<tr color-table-item="${currentColorsCount}">  <td>${colorName}</td> <td style="background:${colorCode}"></td> <td>${colorPrice}</td> <td><a remove-color-btn="${currentColorsCount}" class="btn btn-danger btn-sm text-white" onclick="RemoveProductColore(${currentColorsCount})">حذف رنگ</a></td>  </tr>`;
    $('#list-of-product-colors').prepend(tableNode);
});

const RemoveProductColore = (index) => {
    $(`[color-table-item=${index}]`).remove()
    $(`[color-name-hidden-inp=${index}]`).remove()
    $(`[color-price-hidden-inp=${index}]`).remove()
    $(`[color-code-hidden-inp=${index}]`).remove()

    $(`[color-name-hidden-inp]`).each(function () {
        var thisId = $(this).attr('color-name-hidden-inp');
        if (thisId > index) {
            $(this).attr('color-name-hidden-inp', thisId - 1);
            $(this).attr('name', `ProductColors[${thisId - 1}].ColorName`);
        }
        })
    $(`[color-price-hidden-inp]`).each(function () {
        var thisId = $(this).attr('color-price-hidden-inp');
        if (thisId > index) {
            $(this).attr('color-price-hidden-inp', thisId - 1);
            $(this).attr('name', `ProductColors[${thisId - 1}].Price`);
        }
    })
    $(`[color-code-hidden-inp]`).each(function () {
        var thisId = $(this).attr('color-code-hidden-inp');
        if (thisId > index) {
            $(this).attr('color-code-hidden-inp', thisId - 1);
            $(this).attr('name', `ProductColors[${thisId - 1}].ColorCode`);
        }
    })
    $(`[color-table-item]`).each(function () {
        var thisId = $(this).attr('color-table-item');
        if (thisId > index) $(this).attr('color-table-item', thisId-1);
    })
    $(`[remove-color-btn]`).each(function () {
        var thisId = $(this).attr('remove-color-btn');
        if (thisId > index) $(this).attr('remove-color-btn', thisId - 1);
        if (thisId > index) $(this).attr('onClick', `RemoveProductColore(${thisId - 1})`);
    })
}
function commafy(num) {
    var str = num.toString().split('.');
    if (str[0].length >= 5) {
        str[0] = str[0].replace(/(\d)(?=(\d{3})+$)/g, '$1,');
    }
    if (str[1] && str[1].length >= 5) {
        str[1] = str[1].replace(/(\d{3})/g, '$1 ');
    }
    return str.join('.');
}

//-----------------------------------------------

$("#AddProductFeatureToList").click(function (e) {
    console.log('dj')
    e.preventDefault();
    var featureTitle = $('#product-feature-title-inp').val();
    var featureValue = $('#product-feature-value-inp').val();
    var currentFeaturesCount = $('#list-of-product-features tr').length;
    if (isEmptyOrSpaces(featureTitle)) { return ShowMessage('اخطار', 'عنوان ویژگی را وارد کنید', 'warning') };
    if (isEmptyOrSpaces(featureValue)) { return ShowMessage('اخطار', 'مقددار ویژگی را وارد کنید', 'warning') }
    $('#product-feature-title-inp').val(' ');
    $('#product-feature-value-inp').val(' ');
    var featureTitleNode = `<input type="hidden" value="${featureTitle}" name="ProductFeature[${currentFeaturesCount}].Title" feature-title-hidden-inp="${currentFeaturesCount}"/>`;
    var featureValueeNode = `<input type="hidden" value="${featureValue}" name="ProductFeature[${currentFeaturesCount}].Value" feature-value-hidden-inp="${currentFeaturesCount}"/>`;
    $('#create-product-form').prepend(featureTitleNode);
    $('#create-product-form').prepend(featureValueeNode);
    var tableNode = `<tr feature-table-item="${currentFeaturesCount}">  <td>${featureTitle}</td> <td>${featureValue}</td> <td><a remove-feature-btn="${currentFeaturesCount}" class="btn btn-danger btn-sm text-white" onclick="RemoveProductFeature(${currentFeaturesCount})">حذف ویژگی</a></td>  </tr>`;
    $('#list-of-product-features').prepend(tableNode);
});

const RemoveProductFeature = (index) => {
    $(`[feature-table-item=${index}]`).remove()
    $(`[feature-title-hidden-inp=${index}]`).remove()
    $(`[feature-value-hidden-inp=${index}]`).remove()

    $(`[feature-title-hidden-inp]`).each(function () {
        var thisId = $(this).attr('feature-title-hidden-inp');
        if (thisId > index) {
            $(this).attr('feature-title-hidden-inp', thisId - 1);
            $(this).attr('name', `ProductFeature[${thisId - 1}].Title`);
        }
    })
    $(`[feature-value-hidden-inp]`).each(function () {
        var thisId = $(this).attr('feature-value-hidden-inp');
        if (thisId > index) {
            $(this).attr('feature-value-hidden-inp', thisId - 1);
            $(this).attr('name', `ProductFeature[${thisId - 1}].Value`);
        }
    })
    $(`[feature-table-item]`).each(function () {
        var thisId = $(this).attr('feature-table-item');
        if (thisId > index) $(this).attr('feature-table-item', thisId - 1);
    })
    $(`[remove-feature-btn]`).each(function () {
        var thisId = $(this).attr('remove-feature-btn');
        if (thisId > index) $(this).attr('remove-feature-btn', thisId - 1);
        if (thisId > index) $(this).attr('onClick', `RemoveProductFeature(${thisId-1})`);
    })
}




function changeProductPriceBasedOnColor(price, colorName,colorId) {
    const productBasePrice = $('#product-base-price').val()
    $('.current_price').html(commafy(parseInt(productBasePrice) + price) + ' تومان' + ` ( ${colorName} )`)
    $('#add-product-to-order-productColorId').val(colorId);
}



function onSuccessAddProductToOrder(res) {
    console.log(res)
    ShowMessage('پیغام', res.message, res.status.toLowerCase())
    close_waiting()
}

$('#productCounter').change(function (e) {
    let value = parseInt(e.target.value);
    if (value && value > 0) {
        $('#add-product-to-order-count').val(value);
    }
})

function submitForm(formId) {
    $(formId).submit();
    open_waiting()
}

function removeProductFromOrder(detailId) {
    $.get("/user/remove-order-item/" + detailId)
        .then(res => {
            console.log(res)
            ShowMessage('پیغام', res.message, res.status.toLowerCase())
            if (res.isFinally) setTimeout(() => { window.location.reload() }, 1500)
           // $("#user-open-order-wrapper").html(res.data);
            
        })
        .catch(err => {

        })
}

function changeOrderDetailCount(e, detailId) {
    console.log(e)
    open_waiting()
    $.get("/user/change-detail-count/" + detailId + "/" + e.value)
        .then(res => {
            $("#user-open-order-wrapper").html(res);
            close_waiting();
        })
        .catch(err => {

        })
}