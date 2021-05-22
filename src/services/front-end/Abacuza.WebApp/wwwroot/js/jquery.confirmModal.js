/*!
 * jQuery.confirmModal v1.0
 * Copyright (c) 2018 Trim C.
 * Released under the MIT license
 * Description : simple to use plugin replacing the browser's default confirm box with bootstrap 4 modal
 */
(function ($) {
    $currentModalTarget = {};
    $defaultsConfirmModal = {};
    jQuery.confirmModal = function(message, options, callback) {
        var targetElement = document.activeElement;

        var settings = $.extend({}, $defaultsConfirmModal, options);

        if (settings === undefined) {
            var modalBoxWidth = 'auto';
            var modalVerticalCenter = '';
            var fadeAnimation = '';
            var confirmButton = 'OK';
            var cancelButton = 'Cancel';
        } else {
            if ($defaultsConfirmModal.confirmButton === undefined) {
                var confirmButton = 'OK';
            } else {
                var confirmButton = $defaultsConfirmModal.confirmButton;
            }
            if ($defaultsConfirmModal.cancelButton === undefined) {
                var cancelButton = 'Cancel'; 
            } else {
                var cancelButton = $defaultsConfirmModal.cancelButton;
            }
            if (settings.modalBoxWidth === undefined) {
                var modalBoxWidth = 'auto';
            } else {
                var modalBoxWidth = settings.modalBoxWidth;
            }
            if (settings.modalVerticalCenter === undefined || settings.modalVerticalCenter === false) {
                var modalVerticalCenter = '';
            } else {
                var modalVerticalCenter = 'modal-dialog-centered';
            }
            if (settings.messageHeader === undefined || settings.messageHeader === '') {
                var messageHeader = '&nbsp;';
            } else {
                var messageHeader = settings.messageHeader;
            }
            if (settings.fadeAnimation === undefined || settings.fadeAnimation === false) {
                var fadeAnimation = '';
            } else {
                var fadeAnimation = 'fade';
            }
            if (settings.backgroundBlur === true) {
                $('.container').attr('style', '-webkit-filter: blur(0.1rem); -moz-filter: blur(0.1rem);	-o-filter: blur(0.1rem); -ms-filter: blur(0.1rem); filter: blur(0.1rem);');
                $(document).one('hide.bs.modal', '.modalConfirm', function () { $('.container').removeAttr('style'); });
            } else if (typeof(settings.backgroundBlur) === 'object') {
                if (settings.backgroundBlur.length === 2) {
                    var blurSize = settings.backgroundBlur[1];
                } else {
                    var blurSize = '0.1rem';
                }
                var elements = settings.backgroundBlur[0];                
                $(elements).attr('style', '-webkit-filter: blur(' + blurSize + '); -moz-filter: blur(' + blurSize + '); -o-filter: blur(' + blurSize + '); -ms-filter: blur(' + blurSize + '); filter: blur(' + blurSize + ');');
                $(document).one('hide.bs.modal', '.modalConfirm', function () { $(elements).removeAttr('style'); });
            }
            if (settings.autoFocusOnConfirmBtn === true) {
                $(document).one('shown.bs.modal', '.modalConfirm', function () { $('.confirmButton').focus(); });
            }
        }
        
        var html = `
            <div style="z-index: 5000;" class="modal ` + fadeAnimation + ` modalConfirm" tabindex="-1" role="dialog" aria-labelledby="modal" aria-hidden="true">
                <div style="max-width: ` + modalBoxWidth + `;" class="modal-dialog ` + modalVerticalCenter + `" role="document">
                    <div class="modal-content">
                        <div style="padding: 0.6rem" class="modal-header">
                            <h6 class="modal-title">` + messageHeader + `</h6>
                            <button type="button" style="padding: 0.45rem 0.8rem;" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        </div>
                        <div style="padding: 0.8rem; font-size: 0.9rem;" class="modal-body text-break">` + message + `</div>
                        <div style="padding: 0.55rem; -webkit-touch-callout: none; -webkit-user-select: none; -khtml-user-select: none; -moz-user-select: none; -ms-user-select: none; user-select: none;" class="modal-footer">
                            <button type="button" style="` + (confirmButton === 'OK' ? `width: 66px;` : ``) + ` padding: .18rem .5rem;" class="confirmButton btn btn-primary btn-sm">` + confirmButton + `</button>
                            <button type="button" style="` + (cancelButton === 'Cancel' ? `width: 66px;` : ``) + ` padding: .18rem .5rem;" class="btn btn-secondary btn-sm" data-dismiss="modal">` + cancelButton + `</button>
                        </div>
                    </div>
                </div>
            </div>
        `;

        if ($.isEmptyObject($currentModalTarget)) {
            $currentModalTarget = targetElement;
            $('body').prepend(html);
            $('.modalConfirm').modal('show');
            $('.confirmButton').on('click', function(e) {
                e.preventDefault();
                $('.modalConfirm').modal('hide');
                if (typeof(callback) === 'function') {
                    callback(targetElement);
                } else {
                    options(targetElement)
                }
            });
        } else if ($currentModalTarget.className != targetElement.className) {
            $('body > div.modalConfirm').remove();
            $currentModalTarget = targetElement;
            $('body').prepend(html);
            $('.modalConfirm').modal('show');
            $('.confirmButton').off('click');
            $('.confirmButton').on('click', function(e) {
                e.preventDefault();
                $('.modalConfirm').modal('hide');
                if (typeof(callback) === 'function') {
                    callback(targetElement);
                } else {
                    options(targetElement)
                }
            });
        } else if (targetElement != $currentModalTarget) {
            $currentModalTarget = targetElement;
            $('.modalConfirm').modal('show');
            $('.confirmButton').off('click');
            $('.confirmButton').on('click', function(e) {
                e.preventDefault();
                $('.modalConfirm').modal('hide');
                if (typeof(callback) === 'function') {
                    callback(targetElement);
                } else {
                    options(targetElement)
                }
            });
        } else {
            $('.modalConfirm').modal('show');
        }
    };
}(jQuery));
