// in src/Locations.js
import React from 'react';
import { List, Edit, Create, Datagrid, BooleanInput , TextField, EditButton,  SimpleForm, TextInput, BooleanField } from 'react-admin';

export const LocationList = (props) => (
    <List {...props}>
        <Datagrid>
            <TextField source="description" />
            <TextField source="label" />
            <BooleanField label="Is Active" source="isActive" />
            <EditButton />
        </Datagrid>
    </List>
);

const LocationDescription = ({ record }) => {

    return <span>{record ? `"${record.description}"` : ''}</span>;
};

export const LocationEdit = (props) => (
    <Edit title={<LocationDescription />} {...props}>
        <SimpleForm>
            <TextInput source="description" />
            <TextInput source="label" />
            <BooleanInput label="Is Active" source="isActive" />
        </SimpleForm>
    </Edit>
);

export const LocationCreate = (props) => (
    <Create {...props}>
        <SimpleForm>
            <TextInput source="description" />
            <TextInput source="label" />
            <BooleanInput defaultValue="true" label="Is Active" source="isActive" />
        </SimpleForm>
    </Create>
);