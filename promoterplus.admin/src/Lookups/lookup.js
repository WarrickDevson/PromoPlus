// in src/Lookups.js
import React from 'react';
import { List, Edit, Create, Datagrid, BooleanInput , TextField, EditButton,  SimpleForm, TextInput, BooleanField } from 'react-admin';

const ModifiedUser = ({ record }) => {
    return <span>{record.modifiedUser ? `${record.modifiedUser.name + ' ' + record.modifiedUser.surname}` : ''}</span>;
};

export const LookupList = (props) =>  (
    <List {...props}>
        <Datagrid>
            <TextField source="description" />
            <BooleanField label="Is Active" source="isActive" />
            <ModifiedUser label="Modified User"></ModifiedUser>
            <EditButton />
        </Datagrid>
    </List>
);

const LookupDescription = ({ record,options }) => {
    console.log(options)
    return <span>{record ? `"${record.description}"` : ''}</span>;
};

export const LookupEdit = (props) => (
    <Edit title={<LookupDescription />} {...props}>
        <SimpleForm>
            <TextInput source="description" />
            <BooleanInput label="Is Active" source="isActive" />
        </SimpleForm>
    </Edit>
);

export const LookupCreate = (props) => (
    <Create {...props}>
        <SimpleForm>
            <TextInput source="description" />
            <BooleanInput defaultValue="true" label="Is Active" source="isActive" />
        </SimpleForm>
    </Create>
);