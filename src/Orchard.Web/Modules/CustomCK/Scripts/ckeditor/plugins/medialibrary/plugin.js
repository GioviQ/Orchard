(function () {

    function getUrl(editor) {
        var selection = editor.getSelection(),
			element = selection.getStartElement(),
			insertMode = false;
        if (element)
            element = element.getAscendant('img', true);

        if (!element || element.getName() != 'img' || element.data('cke-realelement')) {
            insertMode = true;
        }

        var baseUrl = editor.config.orchardBaseUrl;
        if (!baseUrl) {
            var adminIndex = location.href.toLowerCase().indexOf("/admin/");
            if (adminIndex === -1) return;
            baseUrl = location.href.substr(0, adminIndex);
        }
        		var url = location.href.substr(0, adminIndex) + "/Admin/Orchard.MediaLibrary?dialog=true";
        //var url = baseUrl + "/Admin/Media Library?dialog=true";
        $.colorbox({
            href: url,
            iframe: true,
            reposition: true,
            width: "90%",
            height: "90%",
            onLoad: function () {
                // hide the scrollbars from the main window
                $('html, body').css('overflow', 'hidden');
            },
            onClosed: function () {
                $('html, body').css('overflow', '');

                var selectedData = $.colorbox.selectedData;

                if (selectedData == null || selectedData.length < 1) {
                    // Dialog cancelled, do nothing
                    return;
                }

                				var renderMedia = location.href.substr(0, adminIndex) + "/Admin/Orchard.MediaLibrary/MediaItem/" + selectedData[0].id + "?displayType=Raw";
                //var renderMedia = baseUrl + "/Admin/Media Library/MediaItem/" + selectedData[0].id + "?displayType=Raw";
                $.ajax({
                    async: false,
                    type: 'GET',
                    url: renderMedia,
                    success: function (data) {
                        var regex = /<img.*?src=['"](.*?)['"]/;
                        var src = regex.exec(data)[1];
                        if (insertMode) {
                            var img = editor.document.createElement('img');
                            img.setAttribute('src', src);
                            editor.insertElement(img);
                        }
                        else {
                            var img = editor.document.createElement('img');
                            var attributes = element.$.attributes;
                            for (var i = 0; i < attributes.length; i++) {
                                if (attributes[i].nodeName.toLowerCase() !== "data-cke-saved-src" && attributes[i].nodeName.toLowerCase() !== "src") {
                                    img.setAttribute(attributes[i].nodeName, attributes[i].nodeValue);
                                }
                            }
                            img.setAttribute('src', src);
                            element.remove();
                            editor.insertElement(img);
                        }
                    }
                });

            }
        })

    }

    CKEDITOR.plugins.add('medialibrary', {
        icons: 'medialibrary',
        init: function (editor) {
            editor.addCommand('mediaLibraryDialog', {
                exec: getUrl
            }
				);
            editor.ui.addButton('MediaLibrary', {
                label: 'Pick from Orchard Media Library',
                command: 'mediaLibraryDialog',
                toolbar: 'insert'
            });

            if (editor.contextMenu) {
                editor.addMenuGroup('abbrGroup');
                editor.addMenuItem('medialibraryItem', {
                    label: 'Pick from Media Library',
                    icon: this.path + 'icons/medialibrary.png',
                    command: 'mediaLibraryDialog',
                    group: 'abbrGroup'
                });
                editor.contextMenu.addListener(function (element) {
                    if (element.getAscendant('img', true)) {
                        return { medialibraryItem: CKEDITOR.TRISTATE_OFF };
                    }
                });
            }
        }
    });

})();