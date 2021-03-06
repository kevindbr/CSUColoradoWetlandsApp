/*
 * File: app/model/Reference.js
 *
 * This file was generated by Sencha Architect version 3.0.4.
 * http://www.sencha.com/products/architect/
 *
 * This file requires use of the Sencha Touch 2.3.x library, under independent license.
 * License of Sencha Architect does not include license for Sencha Touch 2.3.x. For more
 * details see http://www.sencha.com/license or contact license@sencha.com.
 *
 * This file will be auto-generated each and everytime you save your project.
 *
 * Do NOT hand edit this file.
 */

Ext.define('CWIC.model.Reference', {
    extend: 'Ext.data.Model',

    requires: [
        'Ext.data.Field',
        'Ext.data.proxy.Memory',
        'Ext.data.reader.Json'
    ],

    config: {
        idProperty: 'ID',
        fields: [
            {
                name: 'ID',
                type: 'int'
            },
            {
                name: 'Reference',
                type: 'string'
            },
            {
                name: 'FullCitation',
                type: 'string'
            }
        ],
        proxy: {
            type: 'memory',
            reader: {
                type: 'json',
                rootProperty: 'References'
            }
        }
    }
});