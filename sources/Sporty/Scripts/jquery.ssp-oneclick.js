/* 
 * SocialSharePrivacy OneClick
 * 
 * ===========================
 * 
 * BASED ON:
 *
 * jquery.socialshareprivacy.js | 2 Klicks fuer mehr Datenschutz
 *
 * http://www.heise.de/extras/socialshareprivacy/
 * http://www.heise.de/ct/artikel/2-Klicks-fuer-mehr-Datenschutz-1333879.html
 *
 * Copyright (c) 2011 Hilko Holweg, Sebastian Hilbig, Nicolas Heiringhoff, Juergen Schmidt,
 * Heise Zeitschriften Verlag GmbH & Co. KG, http://www.heise.de
 *
 * is released under the MIT License http://www.opensource.org/licenses/mit-license.php
 *
 * Spread the word, link to us if you can.
 */
(function ($) {

    "use strict";

	/*
	 * helper functions
	 */ 

    // abbreviate at last blank before length and add "\u2026" (horizontal ellipsis)
    function abbreviateText(text, length) {
        var abbreviated = decodeURIComponent(text);
        if (abbreviated.length <= length) {
            return text;
        }

        var lastWhitespaceIndex = abbreviated.substring(0, length - 1).lastIndexOf(' ');
        abbreviated = encodeURIComponent(abbreviated.substring(0, lastWhitespaceIndex)) + "\u2026";

        return abbreviated;
    }

    // returns content of <meta name="" content=""> tags or '' if empty/non existant
    function getMeta(name) {
        var metaContent = $('meta[name="' + name + '"]').attr('content');
        return metaContent || '';
    }
    
    // create tweet text from content of <meta name="DC.title"> and <meta name="DC.creator">
    // fallback to content of <title> tag
    function getTweetText() {
        var title = getMeta('DC.title');
        var creator = getMeta('DC.creator');

        if (title.length > 0 && creator.length > 0) {
            title += ' - ' + creator;
        } else {
            title = $('title').text();
        }

        return encodeURIComponent(title);
    }

    // build URI from rel="canonical" or document.location
    function getURI() {
        var uri = document.location.href;
        var canonical = $("link[rel=canonical]").attr("href");

        if (canonical && canonical.length > 0) {
            if (canonical.indexOf("http") < 0) {
                canonical = document.location.protocol + "//" + document.location.host + canonical;
            }
            uri = canonical;
        }

        return uri;
    }

    function cookieSet(name, value, days, path, domain) {
        var expires = new Date();
        expires.setTime(expires.getTime() + (days * 24 * 60 * 60 * 1000));
        document.cookie = name + '=' + value + '; expires=' + expires.toUTCString() + '; path=' + path + '; domain=' + domain;
    }
    function cookieDel(name, value, path, domain) {
        var expires = new Date();
        expires.setTime(expires.getTime() - (7 * 24 * 60 * 60 * 1000));
        document.cookie = name + '=' + value + '; expires=' + expires.toUTCString() + '; path=' + path + '; domain=' + domain;
    }

    function activateButton(container, dummy_img, code) {
        container
            .addClass('info_off')
            .find(dummy_img).replaceWith(code);
    }
    function deactivateButton(container, btn_class, dummy_btn) {
        container
            .removeClass('info_off')
            .find(btn_class).html(dummy_btn);
    }

    // extend jquery with our plugin function
    $.fn.socialSharePrivacy = function (settings) {
        var defaults = {
            'services' : {
                'facebook' : {
                    'status'            : 'on',
                    'dummy_img'         : 'sspoc/img/dummy_facebook.png',
                    'txt_info'          : '2 Klicks f&uuml;r mehr Datenschutz: Erst wenn Sie hier klicken, wird der Button aktiv und Sie k&ouml;nnen Ihre Empfehlung an Facebook senden. Schon beim Aktivieren werden Daten an Dritte &uuml;bertragen &ndash; siehe <em>i</em>.',
                    'txt_fb_off'        : 'nicht mit Facebook verbunden',
                    'txt_fb_on'         : 'mit Facebook verbunden',
                    'perma_option'      : 'on',
                    'display_name'      : 'Facebook',
                    'referrer_track'    : '',
                    'language'          : 'de_DE',
                    'action'            : 'recommend'
                }, 
                'twitter' : {
                    'status'            : 'on', 
                    'dummy_img'         : 'sspoc/img/dummy_twitter.png',
                    'txt_info'          : '2 Klicks f&uuml;r mehr Datenschutz: Erst wenn Sie hier klicken, wird der Button aktiv und Sie k&ouml;nnen Ihre Empfehlung an Twitter senden. Schon beim Aktivieren werden Daten an Dritte &uuml;bertragen &ndash; siehe <em>i</em>.',
                    'txt_twitter_off'   : 'nicht mit Twitter verbunden',
                    'txt_twitter_on'    : 'mit Twitter verbunden',
                    'perma_option'      : 'on',
                    'display_name'      : 'Twitter',
                    'referrer_track'    : '', 
                    'tweet_text'        : getTweetText,
                    'language'          : 'en'
                },
                'gplus' : {
                    'status'            : 'on',
                    'dummy_img'         : 'sspoc/img/dummy_gplus.png',
                    'txt_info'          : '2 Klicks f&uuml;r mehr Datenschutz: Erst wenn Sie hier klicken, wird der Button aktiv und Sie k&ouml;nnen Ihre Empfehlung an Google+ senden. Schon beim Aktivieren werden Daten an Dritte &uuml;bertragen &ndash; siehe <em>i</em>.',
                    'txt_gplus_off'     : 'nicht mit Google+ verbunden',
                    'txt_gplus_on'      : 'mit Google+ verbunden',
                    'perma_option'      : 'on',
                    'display_name'      : 'Google+',
                    'referrer_track'    : '',
                    'language'          : 'de'
                }
            },
            'info_link'         : 'http://www.heise.de/ct/artikel/2-Klicks-fuer-mehr-Datenschutz-1333879.html',
            'txt_help'          : 'Wenn Sie diese Felder durch einen Klick aktivieren, werden Informationen an Facebook, Twitter oder Google in die USA &uuml;bertragen und unter Umst&auml;nden auch dort gespeichert. N&auml;heres erfahren Sie durch einen Klick auf das <em>i</em>.',
            'settings_perma'    : 'Dauerhaft aktivieren und Daten&uuml;ber&shy;tragung zustimmen:',
            'cookie_path'       : '/',
            'cookie_domain'     : document.location.host,
            'cookie_expires'    : '365',
            'css_path'          : 'sspoc/ssp-oneclick.css',
            'uri'               : getURI,
            'txt_activate_all'  : 'Share-Buttons aktivieren',
            'txt_deactivate_all': 'Share-Buttons deaktivieren'
        };

        // Standardwerte des Plug-Ings mit den vom User angegebenen Optionen ueberschreiben
        var options = $.extend(true, defaults, settings);

        var facebook_on = (options.services.facebook.status === 'on');
        var twitter_on  = (options.services.twitter.status  === 'on');
        var gplus_on    = (options.services.gplus.status    === 'on');

        // check if at least one service is "on"
        if (!facebook_on && !twitter_on && !gplus_on) {
            return;
        }

        // insert stylesheet into document and prepend target element
        if (options.css_path.length > 0) {
            // IE fix (noetig fuer IE < 9 - wird hier aber fuer alle IE gemacht)
            if (document.createStyleSheet) {
                document.createStyleSheet(options.css_path);
            } else {
                $('head').append('<link rel="stylesheet" type="text/css" href="' + options.css_path + '" />');
            }
        }

        return this.each(function () {

            $(this).prepend('<ul class="ssp_area"></ul>');
            var context = $('ul.ssp_area', this);

            // canonical uri that will be shared
            var uri = options.uri;
            if (typeof uri === 'function') {
                uri = uri(context);
            }

            //
            // Facebook
            //
            if (facebook_on) {
                var fb_enc_uri = encodeURIComponent(uri + options.services.facebook.referrer_track);
                var fb_code = '<iframe src="http://www.facebook.com/plugins/like.php?locale=' + options.services.facebook.language + '&amp;href=' + fb_enc_uri + '&amp;send=false&amp;layout=button_count&amp;width=120&amp;show_faces=false&amp;action=' + options.services.facebook.action + '&amp;colorscheme=light&amp;font&amp;height=21" scrolling="no" frameborder="0" style="border:none; overflow:hidden; width:145px; height:21px;" allowTransparency="true"></iframe>';
                var fb_dummy_btn = '<img src="' + options.services.facebook.dummy_img + '" alt="Facebook &quot;Like&quot;-Dummy" class="fb_dummy dummy_img" data-service="fb" />';

                context.append('<li class="facebook help_info"><span class="info">' + options.services.facebook.txt_info + '</span><div class="fb_like dummy_btn">' + fb_dummy_btn + '</div></li>');

                var $container_fb = $('li.facebook', context);
            }

            //
            // Twitter
            //
            if (twitter_on) {
                var text = options.services.twitter.tweet_text;
                if (typeof text === 'function') {
                    text = text();
                }
                // 120 is the max character count left after twitters automatic url shortening with t.co
                text = abbreviateText(text, '120');

                var twitter_enc_uri = encodeURIComponent(uri + options.services.twitter.referrer_track);
                var twitter_count_url = encodeURIComponent(uri);
                var twitter_code = '<iframe allowtransparency="true" frameborder="0" scrolling="no" src="http://platform.twitter.com/widgets/tweet_button.html?url=' + twitter_enc_uri + '&amp;counturl=' + twitter_count_url + '&amp;text=' + text + '&amp;count=horizontal&amp;lang=' + options.services.twitter.language + '" style="width:100px; height:25px;"></iframe>';
                var twitter_dummy_btn = '<img src="' + options.services.twitter.dummy_img + '" alt="&quot;Tweet this&quot;-Dummy" class="twitter_dummy dummy_img" data-service="twitter" />';

                context.append('<li class="twitter help_info"><span class="info">' + options.services.twitter.txt_info + '</span><div class="tweet dummy_btn">' + twitter_dummy_btn + '</div></li>');

                var $container_tw = $('li.twitter', context);
            }

            //
            // Google+
            //
            if (gplus_on) {
                // fuer G+ wird die URL nicht encoded, da das zu einem Fehler fuehrt
                var gplus_uri = uri + options.services.gplus.referrer_track;
                
                // we use the Google+ "asynchronous" code, standard code is flaky if inserted into dom after load
                var gplus_code = '<div class="g-plusone" data-size="medium" data-href="' + gplus_uri + '"></div><script type="text/javascript">window.___gcfg = {lang: "' + options.services.gplus.language + '"}; (function() { var po = document.createElement("script"); po.type = "text/javascript"; po.async = true; po.src = "https://apis.google.com/js/plusone.js"; var s = document.getElementsByTagName("script")[0]; s.parentNode.insertBefore(po, s); })(); </script>';
                var gplus_dummy_btn = '<img src="' + options.services.gplus.dummy_img + '" alt="&quot;Google+1&quot;-Dummy" class="gplus_dummy dummy_img" data-service="gplus" />';

                context.append('<li class="gplus help_info"><span class="info">' + options.services.gplus.txt_info + '</span><div class="gplusone dummy_btn">' + gplus_dummy_btn + '</div></li>');

                var $container_gplus = $('li.gplus', context);
            }

            // Einzelne Knöpfe
            $(context).on('click', 'img.dummy_img', function(event) {
                switch ($(event.target).data('service')) {
                    case 'fb':        activateButton($container_fb, 'img.fb_dummy', fb_code); break;
                    case 'twitter':   activateButton($container_tw, 'img.twitter_dummy', twitter_code); break;
                    case 'gplus':     activateButton($container_gplus, 'img.gplus_dummy', gplus_code); break;
                }
            });

            // Alle-aktivieren-Knoppes
            var $btn = $('<span class="off btn">')
                .html(options.txt_activate_all)
                .click(function(){
                    if ($(this).hasClass('off')) {
                        if (facebook_on)    activateButton($container_fb, 'img.fb_dummy', fb_code);
                        if (twitter_on)     activateButton($container_tw, 'img.twitter_dummy', twitter_code);
                        if (gplus_on)       activateButton($container_gplus, 'img.gplus_dummy', gplus_code);

                        $(this)
                            .removeClass('off')
                            .html(options.txt_deactivate_all);
                    }
                    else {
                        if (facebook_on)    deactivateButton($container_fb, '.fb_like', fb_dummy_btn);                        
                        if (twitter_on)     deactivateButton($container_tw, '.tweet', twitter_dummy_btn); 
                        if (gplus_on)       deactivateButton($container_gplus, '.gplusone', gplus_dummy_btn); 

                        $(this)
                            .addClass('off')
                            .html(options.txt_activate_all);
                    }
                });

            // Der Info/Settings-Bereich wird eingebunden
            context.append($('<li class="settings_info">')
                .append($('<div class="settings_info_menu off perma_option_off">')
                    .append($btn)
                    .append('<a href="' + options.info_link + '"><span class="help_info icon"><span class="info">' + options.txt_help + '</span></span></a>')
                )
            );

            // Info-Overlays mit leichter Verzoegerung einblenden
            $('.help_info:not(.info_off)', context).on('mouseenter', function () {
                var $info_wrapper = $(this);
                var timeout_id = window.setTimeout(function () { $($info_wrapper).addClass('display'); }, 250);
                $(this).data('timeout_id', timeout_id);
            });
            $('.help_info', context).on('mouseleave', function () {
                var timeout_id = $(this).data('timeout_id');
                window.clearTimeout(timeout_id);
                if ($(this).hasClass('display')) {
                    $(this).removeClass('display');
                }
            });

            var facebook_perma = (options.services.facebook.perma_option === 'on');
            var twitter_perma  = (options.services.twitter.perma_option  === 'on');
            var gplus_perma    = (options.services.gplus.perma_option    === 'on');

            // Menue zum dauerhaften Einblenden der aktiven Dienste via Cookie einbinden
            // Die IE7 wird hier ausgenommen, da er kein JSON kann und die Cookies hier ueber JSON-Struktur abgebildet werden
            if (((facebook_on && facebook_perma)
                || (twitter_on && twitter_perma)
                || (gplus_on && gplus_perma))
                    && (JSON && typeof JSON.parse === 'function')) {

                // Cookies abrufen
                var cookie_list = document.cookie.split(';');
                var cookies = '{';
                var i = 0;
                for (; i < cookie_list.length; i += 1) {
                    var foo = cookie_list[i].split('=');
                    cookies += '"' + $.trim(foo[0]) + '":"' + $.trim(foo[1]) + '"';
                    if (i < cookie_list.length - 1) {
                        cookies += ',';
                    }
                }
                cookies += '}';
                cookies = JSON.parse(cookies);

                // Container definieren
                var $container_settings_info = $('li.settings_info', context);

                // Klasse entfernen, die das i-Icon alleine formatiert, da Perma-Optionen eingeblendet werden
                $container_settings_info.find('.settings_info_menu').removeClass('perma_option_off');

                // Perma-Optionen-Icon (.settings) und Formular (noch versteckt) einbinden
                $container_settings_info.find('.settings_info_menu').append('<span class="settings">Einstellungen</span><form><fieldset><legend>' + options.settings_perma + '</legend></fieldset></form>');


                // Die Dienste mit <input> und <label>, sowie checked-Status laut Cookie, schreiben
                var checked = ' checked="checked"';
                if (facebook_on && facebook_perma) {
                    var perma_status_facebook = cookies.socialSharePrivacy_facebook === 'perma_on' ? checked : '';
                    $container_settings_info.find('form fieldset').append(
                        '<input type="checkbox" name="perma_status_facebook" id="perma_status_facebook"'
                            + perma_status_facebook + ' /><label for="perma_status_facebook">'
                            + options.services.facebook.display_name + '</label>'
                    );
                }

                if (twitter_on && twitter_perma) {
                    var perma_status_twitter = cookies.socialSharePrivacy_twitter === 'perma_on' ? checked : '';
                    $container_settings_info.find('form fieldset').append(
                        '<input type="checkbox" name="perma_status_twitter" id="perma_status_twitter"'
                            + perma_status_twitter + ' /><label for="perma_status_twitter">'
                            + options.services.twitter.display_name + '</label>'
                    );
                }

                if (gplus_on && gplus_perma) {
                    var perma_status_gplus = cookies.socialSharePrivacy_gplus === 'perma_on' ? checked : '';
                    $container_settings_info.find('form fieldset').append(
                        '<input type="checkbox" name="perma_status_gplus" id="perma_status_gplus"'
                            + perma_status_gplus + ' /><label for="perma_status_gplus">'
                            + options.services.gplus.display_name + '</label>'
                    );
                }

                // Cursor auf Pointer setzen fuer das Zahnrad
                $container_settings_info.find('span.settings').css('cursor', 'pointer');

                // Einstellungs-Menue bei mouseover ein-/ausblenden
                $($container_settings_info.find('span.settings'), context).on('mouseenter', function () {
                    var timeout_id = window.setTimeout(function () { $container_settings_info.find('.settings_info_menu').removeClass('off').addClass('on'); }, 500);
                    $(this).data('timeout_id', timeout_id);
                }); 
                $($container_settings_info, context).on('mouseleave', function () {
                    var timeout_id = $(this).data('timeout_id');
                    window.clearTimeout(timeout_id);
                    $container_settings_info.find('.settings_info_menu').removeClass('on').addClass('off');
                });

                // Klick-Interaktion auf <input> um Dienste dauerhaft ein- oder auszuschalten (Cookie wird gesetzt oder geloescht)
                $($container_settings_info.find('fieldset input')).on('click', function (event) {
                    var click = event.target.id;
                    var service = click.substr(click.lastIndexOf('_') + 1, click.length);
                    var cookie_name = 'socialSharePrivacy_' + service;

                    if ($(event.target).is(':checked')) {
                        cookieSet(cookie_name, 'perma_on', options.cookie_expires, options.cookie_path, options.cookie_domain);
                        $('form fieldset label[for=' + click + ']', context).addClass('checked');
                    } else {
                        cookieDel(cookie_name, 'perma_on', options.cookie_path, options.cookie_domain);
                        $('form fieldset label[for=' + click + ']', context).removeClass('checked');
                    }
                });

                // Dienste automatisch einbinden, wenn entsprechendes Cookie vorhanden ist
                if (facebook_on && facebook_perma && cookies.socialSharePrivacy_facebook === 'perma_on') {
                    activateButton($container_fb, 'img.fb_dummy', fb_code);
                }
                if (twitter_on && twitter_perma && cookies.socialSharePrivacy_twitter === 'perma_on') {
                    activateButton($container_tw, 'img.twitter_dummy', twitter_code);
                }
                if (gplus_on && gplus_perma && cookies.socialSharePrivacy_gplus === 'perma_on') {
                    activateButton($container_gplus, 'img.gplus_dummy', gplus_code);
                }
            }
        }); // this.each(function ()
    };      // $.fn.socialSharePrivacy = function (settings) {
}(jQuery));

