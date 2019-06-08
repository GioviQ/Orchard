/* global CKEDITOR*/
//use decimal entities for the basic named entities
//copied from the orginal cksource entities plugin. Replaced function getentitiy in order to return always a decimal entity
(function () {
    // Basic HTML entities.
    var htmlbase = 'nbsp,gt,lt,amp';

    // Create a mapping table between one character and its entity form from a list of entity names.
    // @param reverse {Boolean} Whether to create a reverse map from the entity string form to an actual character.
    function buildTable(entities, reverse) {
        var table = {},
			regex = [];

        // Entities that the browsers' DOM does not automatically transform to the
        // final character.
        var specialTable = {
            nbsp: '\u00A0', // IE | FF
            shy: '\u00AD', // IE
            gt: '\u003E', // IE | FF |   --   | Opera
            lt: '\u003C', // IE | FF | Safari | Opera
            amp: '\u0026', // ALL
            apos: '\u0027', // IE
            quot: '\u0022' // IE
        };

        entities = entities.replace(/\b(nbsp|shy|gt|lt|amp|apos|quot)(?:,|$)/g, function (match, entity) {
            var org = reverse ? '&' + entity + ';' : specialTable[entity],
				result = reverse ? specialTable[entity] : '&' + entity + ';';

            table[org] = result;
            regex.push(org);
            return '';
        });

        if (!reverse && entities) {
            // Transforms the entities string into an array.
            entities = entities.split(',');

            // Put all entities inside a DOM element, transforming them to their
            // final characters.
            var div = document.createElement('div'),
				chars;
            div.innerHTML = '&' + entities.join(';&') + ';';
            chars = div.innerHTML;
            div = null;

            // Add all characters to the table.
            for (var i = 0; i < chars.length; i++) {
                var charAt = chars.charAt(i);
                table[charAt] = '&' + entities[i] + ';';
                regex.push(charAt);
            }
        }

        table.regex = regex.join(reverse ? '|' : '');

        return table;
    }

    CKEDITOR.plugins.add('customckentities', {
        afterInit: function (editor) {
           
            function getChar(character) {
                return baseEntitiesTable[character];
            }
            //this function is different from the original entities plugin. It always returns the decimal entitiy
            function getEntity(character) {
                return '&#' + character.charCodeAt(0) + ';';
            }

            var dataProcessor = editor.dataProcessor,
				htmlFilter = dataProcessor && dataProcessor.htmlFilter;

            if (htmlFilter) {
                // Mandatory HTML basic entities.
                var selectedEntities = [];

                selectedEntities.push(htmlbase);                   

                var entitiesTable = buildTable(selectedEntities.join(','));

                // Create the Regex used to find entities in the text, leave it matches nothing if entities are empty.
                var entitiesRegex = entitiesTable.regex ? '[' + entitiesTable.regex + ']' : 'a^';
                delete entitiesTable.regex;

                entitiesRegex = new RegExp(entitiesRegex, 'g');

                // Decode entities that the browsers has transformed
                // at first place.
                var baseEntitiesTable = buildTable([htmlbase, 'shy'].join(','), true),
					baseEntitiesRegex = new RegExp(baseEntitiesTable.regex, 'g');

                htmlFilter.addRules({
                    text: function (text) {
                        return text.replace(baseEntitiesRegex, getChar).replace(entitiesRegex, getEntity);
                    }
                }, {
                    applyToAll: true,
                    excludeNestedEditable: true
                });
            }
        }
    });
})();
