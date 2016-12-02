/*
 * Project: APP for PetSystem
 * Description: Tools for Modal, Ajax e Notify
 * Author: Afonso Dutra Nogueira
 * License: MIT License
 */


function ModalAlert(msg) {
    jQuery('#myModalAlertHTML').html(msg);
    jQuery('#myModalAlert').modal('show');
    jQuery('#myModalAlert').data('bs.modal').handleUpdate();
    return false;
}

function NotifyAlert(msg, type) {
    var Notify = jQuery('.bottom-right').notify(
            {
                message: { html: true, text: msg },
                type: type
            });
    Notify.show(); // for the ones that aren't closable and don't fade out there is a .hide() function.
    return false;
}

function AjaxHTML(url) {
    return jQuery.ajax({
        type: 'GET',
        url: url,
        dataType: 'html',
        async: false,
        success: function (result, status, xhr) {
            return result;
        },
        error: function (xhr, status, error) {
            alert(error);
        }
    });
}

function AjaxJSON(url, parametros) {
    var retorno = "";
    retorno = jQuery.ajax({
        type: 'POST',
        url: url,
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        data: parametros,
        async: false,
        success: function (result, status, xhr) {
            return result.d;
        },
        error: function (xhr, status, error) {
            alert(error);
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
