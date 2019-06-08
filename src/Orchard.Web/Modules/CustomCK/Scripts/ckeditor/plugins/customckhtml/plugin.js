/*global CKEDITOR */
(function () {

    function initAwesomeList(editor) {
       
        CKEDITOR.dtd.$removeEmpty.i = false;//Alow empty <i> </i> in order to ensure that FontAwesome List symbols do not get erased
        editor.addCommand('insertAwesomeList', {

            allowedContent: 'i(fa,fa-check,color-green); ul(list-unstyled,margin-bottom-20)',

            exec: function (editor) {
                editor.insertHtml("<ul class='list-unstyled margin-bottom-20'>" +
                                "<li><i class='fa fa-check color-green'></i>&#160;Eins</li>" +
                                "<li><i class='fa fa-check color-green'></i>&#160;Zwei</li>" +
                                "<li><i class='fa fa-check color-green'></i>&#160;Drei</li>" +
                                "</ul>");
            }
        });

        editor.ui.addButton('AwesomeList', {
            label: 'Insert Awesome List',
            command: 'insertAwesomeList',
            toolbar: 'insert'
        });
    }

    function initUnderlinedH2(editor) {
        editor.addCommand('insertUnderlinedH2', {

            allowedContent: 'div(headline)',

            exec: function (editor) {

                //var selectedText = editor.getSelection().getNative();
                //if (selectedText === "") {
                //    selectedText = "New Headline";
                //}
                editor.insertHtml("<div class='headline'>" +
                                "<h2>" + "New Headline" + "</h2>" +
                                "</div>");
            }
        });

        editor.ui.addButton('Underlinedh2', {
            label: 'Insert Underlined Heading 2',
            command: 'insertUnderlinedH2',
            toolbar: 'insert'
        });
    }

    function initUnderlinedH3(editor) {
        editor.addCommand('insertUnderlinedH3', {

            allowedContent: 'div(headline)',

            exec: function (editor) {

                //var selectedText = editor.getSelection().getNative();
                //if (selectedText === "") {
                //    selectedText = "New Headline";
                //}
                editor.insertHtml("<div class='headline'>" +
                                "<h3>" + "New Headline" + "</h3>" +
                                "</div>");
            }
        });

        editor.ui.addButton('Underlinedh3', {
            label: 'Insert Underlined Heading 3',
            command: 'insertUnderlinedH3',
            toolbar: 'insert'
        });
    }

    function initResponsiveYoutube(editor) {

        editor.addCommand('insertResponsiveYoutube', {

            allowedContent: 'div(flex-video,widescreen); iframe[src,frameborder,allowfullscreen]',

            exec: function (editor) {

                editor.insertHtml("<div class='flex-video widescreen'><iframe src='http://www.youtube.com/embed/kwPvSAKU7fA?showsearch=0&amp;modestbranding=1&amp;rel=0&amp;wmode=opaque' frameborder='0' allowfullscreen='true'></iframe></div>");
            }
        });

        editor.ui.addButton('ResponsiveYoutube', {
            label: 'Insert Responsive Youtube',
            command: 'insertResponsiveYoutube',
            toolbar: 'insert'
        });
    }

    function initTimelineEvent(editor) {

        editor.addCommand('insertTimelineEvent', {

            allowedContent: 'ul(timeline-v2); time(cbp_tmtime); span; div(cbp_tmlabel); i(cbp_tmicon,rounded-x,hidden-xs);',

            //exec: function (editor) {

            //    editor.insertHtml('<li>' +
            //            '<time class="cbp_tmtime">' +
            //                '<span>' +
            //                    '2015' +
            //                '</span>' +
            //            '</time>' +
            //            '<i class="cbp_tmicon rounded-x hidden-xs"></i>' +
            //            '<div class="cbp_tmlabel">' +
            //                '<h2>Label Title</h2>' +
            //                '<p>Label Description</p>' +
            //            '</div>' +     
            //     '</li>');
            //}
        });

        //editor.ui.addButton('TimelineEvent', {
        //    label: 'Insert Timeline Event',
        //    command: 'insertTimelineEvent',
        //    toolbar: 'insert'
        //});
    }

    CKEDITOR.plugins.add('customckhtml', {
        icons: 'AwesomeList,UnderlinedH2,UnderlinedH3,ResponsiveYoutube,TimelineEvent',
        init: function (editor) {

            initAwesomeList(editor);

            initUnderlinedH2(editor);

            initUnderlinedH3(editor);

            initResponsiveYoutube(editor);

            initTimelineEvent(editor);

        },
        afterInit: function (editor) {
            var dataProcessor = editor.dataProcessor;
            var htmlFilter = dataProcessor && dataProcessor.htmlFilter;

            if (htmlFilter) {
                htmlFilter.addRules({
                    text: function (text) {
                        return text.replace('/&nbsp;/', '&#160;');
                    }
                }, {
                    applyToAll: true,
                    excludeNestedEditable: true
                });
            }
        }
    });

})();
