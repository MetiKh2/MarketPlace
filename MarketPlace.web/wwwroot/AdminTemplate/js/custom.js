
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

function FillPageId(pageId) {
$("#PageId").val(pageId);
$("#filter-form").submit();
}

$('[ajax-url-button]').on('click', function (e) {
    e.preventDefault();
    var url = $(this).attr('href');

    var itemId = $(this).attr('ajax-url-button');
    swal({
        title: 'اخطار',
        text: "آیا از انجام عملیات مورد نظر اطمینان دارید؟",
        type: "warning",
        showCancelButton: true,
        confirmButtonClass: "btn-danger",
        confirmButtonText: "بله",
        cancelButtonText: "خیر",
        closeOnConfirm: false,
        closeOnCancel: false
    }).then((result) => {
        if (result.value) {
            $.get(url).then(result => {
                if (result.status === 'Success') {
                    ShowMessage('موفقیت', result.message);
                    $('#ajax-url-' + itemId).hide(1500);
                }
            });
        } else if (result.dismiss === swal.DismissReason.cancel) {
            swal('اعلام', 'عملیات لغو شد', 'error');
        }
    });
   
})

function OnSuccessRejectItem(res) {
    
    if (res.status === 'Success')
        ShowMessage("موفقیت", res.message)
    else
        ShowMessage('شکست', res.message, 'error')
    if (res.isFinally) setTimeout(() => { window.location.reload() }, 1700)

}
 