/*
 * Project: Tools for PetSystem
 * Description: Tools for Modal, Ajax e Notify
 * Author: Afonso Dutra Nogueira
 * License: MIT License
 */

function convertDate(inputFormat) {
    function pad(s) { return (s < 10) ? '0' + s : s; }
    var d = new Date(inputFormat);
    return [pad(d.getDate()), pad(d.getMonth() + 1), d.getFullYear()].join('/');
}

function pageWidth() {
    return window.innerWidth != null ? window.innerWidth : document.documentElement && document.documentElement.clientWidth ? document.documentElement.clientWidth : document.body != null ? document.body.clientWidth : null;
}
function pageHeight() {
    return window.innerHeight != null ? window.innerHeight : document.documentElement && document.documentElement.clientHeight ? document.documentElement.clientHeight : document.body != null ? document.body.clientHeight : null;
}
function posLeft() {
    return typeof window.pageXOffset != 'undefined' ? window.pageXOffset : document.documentElement && document.documentElement.scrollLeft ? document.documentElement.scrollLeft : document.body.scrollLeft ? document.body.scrollLeft : 0;
}
function posTop() {
    return typeof window.pageYOffset != 'undefined' ? window.pageYOffset : document.documentElement && document.documentElement.scrollTop ? document.documentElement.scrollTop : document.body.scrollTop ? document.body.scrollTop : 0;
}

if (typeof (Sys) !== 'undefined') {
    var prm = Sys.WebForms.PageRequestManager.getInstance();
    if (prm != null) {
        prm.add_beginRequest(BeginAjaxHandler);
        prm.add_endRequest(EndAjaxHandler);
    }
}

// ReSharper disable once UnusedParameter
function BeginAjaxHandler(sender, args) {
    UpdateShow();
}
// ReSharper disable once UnusedParameter
function EndAjaxHandler(sender, args) {
    UpdateHide();
}

jQuery(document)
  .ajaxStart(function () {
      UpdateShow();
  })
  .ajaxStop(function () {
      UpdateHide();
  });

function UpdateShow() {
    jQuery('#updateProgress-img').css({ left: (pageWidth() - jQuery('#updateProgress-img').outerWidth()) / 2, top: ((pageHeight() - jQuery('#updateProgress-img').outerHeight()) / 2) + posTop() });
    jQuery('#updateProgress').css({ position: 'absolute', display: 'block' });    
}

function UpdateHide() {
    jQuery('#updateProgress-img').css({ left: (pageWidth() - jQuery('#updateProgress-img').outerWidth()) / 2, top: ((pageHeight() - jQuery('#updateProgress-img').outerHeight()) / 2) + posTop() });
    jQuery('#updateProgress').css({ position: 'absolute', display: 'none' });
    jQuery('body').css({ 'background-color': "#F1F3FA !important" });
}

function ModalAlert(msg) {
    jQuery('#myModalAlertHTML').html(msg);
    jQuery('#myModalAlert').modal('show');
    jQuery('#myModalAlert').data('bs.modal').handleUpdate();
    jQuery("#myModalAlert").on("hidden.bs.modal", function () {jQuery('body').css({'background-color': "#F1F3FA !important"});});
    return false;
}

function NotifySuccess(message, url) {
    return NotifyAlert("", message, "success", "glyphicon glyphicon-ok-sign", url);
}
function NotifyInfo(message, url) {
    return NotifyAlert("", message, "info", "glyphicon glyphicon-exclamation-sign", url);
}
function NotifyWarning(message, url) {
    return NotifyAlert("", message, "warning", "glyphicon glyphicon-warning-sign", url);
}
function NotifyError(message, url) {
    return NotifyAlert("", message, "danger", "glyphicon glyphicon-remove-sign", url);
}

function NotifyImage(title, message, type, urlImg, url) {
    jQuery(document).ready(function () {
        jQuery.notify({
            icon: urlImg,
            title: title,
            message: message,
            url: url,
            target: '_self'
        }, {
            icon_type: 'image',
            type: type,
            mouse_over: 'pause'
        });
        return true;
    });
}

function NotifyAlert(title, message, type, icon, url) {
    jQuery(document).ready(function () {
        jQuery.notify({
            icon: icon,
            title: title,
            message: message,
            url: url,
            target: '_self'
        }, {
            type: type,
            mouse_over: 'pause'
        });
        return true;
    });
}

function ModalAjax(title, url) {
    AjaxHTML('myModalInfoHTML', url);
    jQuery('#myModalLabelHTML').html(title);
    jQuery('#modal-dialog').css({ width: "auto", "min-width": "700px", "max-width": "850px" });
    jQuery('#myModalInfo').modal('show');
    jQuery('#myModalInfo').data('bs.modal').handleUpdate();
    jQuery("#myModalInfo").on("hidden.bs.modal", function () { jQuery('body').css({ 'background-color': "#F1F3FA !important" }); });
    return false;
}

// ReSharper disable once UnusedParameter
function AjaxHTML(objId, url) {
    UpdateShow();
    jQuery('#' + objId).html("<img src='/Images/loader3.gif'/> Aguarde...");
    var htmlResult;
    jQuery.ajax({
        type: 'GET',
        url: url,
        dataType: 'html',
        async: true,
        success: function (result, status, xhr) {
            htmlResult = result;
            UpdateHide();
        },
        error: function (xhr, status, error) {
            UpdateHide();
            NotifyError(error, null);
        },
        complete: function () {
            UpdateHide();
            jQuery('#' + objId).html(htmlResult);
        }
    });
}

// ReSharper disable once UnusedParameter
function AjaxJSON(url, parametros) {
    UpdateShow();
    var retorno = jQuery.ajax({
        type: 'POST',
        url: url,
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        data: parametros,
        async: false,
        success: function (result, status, xhr) {
            UpdateHide();
            return result.d;
        },
        error: function (xhr, status, error) {
            UpdateHide();
            NotifyError(error, null);
        }
    });

    if (retorno.responseText != null) {
        var obj = JSON.parse(retorno.responseText);
        if (obj.d != null && obj.d != "") {
            if (Object.prototype.toString.call(obj.d) === '[object Array]')
                return obj.d;
            var objRetorno = JSON.parse(obj.d);
            return objRetorno;
        } else {
            return null;
        }
    }
    return null;
}
