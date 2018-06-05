// in src/Promoters.js
import React from 'react';
import { List, Edit, Create, Datagrid, BooleanInput , TextField, EditButton,  SimpleForm, TextInput, BooleanField } from 'react-admin';

export const PromoterList = (props) => (
    <List {...props}>
        <Datagrid>
            <TextField source="name" />
            <TextField source="username" />
            <BooleanField label="Is Active" source="isActive" />
            <EditButton />
        </Datagrid>
    </List>
);

const PromoterDescription = ({ record }) => {
    return <span>{record ? `"${record.name}"` : ''}</span>;
};

export const PromoterEdit = (props) => (
    <Edit title={<PromoterDescription />} {...props}>
        <SimpleForm>
        <TextInput source="name" />
            <TextInput source="username" />
            <BooleanInput label="Is Active" source="isActive" />
        </SimpleForm>
    </Edit>
);

export const PromoterCreate = (props) => (
    <Create {...props}>
        <SimpleForm>
        <TextInput source="name" />
            <TextInput source="username" />
            <BooleanInput defaultValue="true" label="Is Active" source="isActive" />
        </SimpleForm>
    </Create>
);