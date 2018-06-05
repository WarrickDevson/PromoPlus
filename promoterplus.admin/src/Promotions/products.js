// in src/Products.js
import React from 'react';
import { List, Edit, Create, Datagrid, BooleanInput , TextField, EditButton,  SimpleForm, TextInput, BooleanField,ReferenceInput,SelectInput } from 'react-admin';

export const ProductList = (props) => (
    <List {...props}>
        <Datagrid>
            <ClientField label="Client"></ClientField>
            <TextField source="description" />
            <TextField source="label" />
            <BooleanField label="Is Active" source="isActive" />
            <EditButton />
        </Datagrid>
    </List>
);

const ClientField = ({ record }) => {
    return <span>{record.client ? `${record.client.description}` : ''}</span>;
};

const ProductDescription = ({ record }) => {

    return <span>{record ? `"${record.description}"` : ''}</span>;
};

export const ProductEdit = (props) => (
    <Edit title={<ProductDescription />} {...props}>
        <SimpleForm>
            <ReferenceInput label="Client" source="clientId" reference="clients">
                <SelectInput optionText="description" />
            </ReferenceInput>
            <TextInput source="description" />
            <TextInput source="label" />
            <BooleanInput label="Is Active" source="isActive" />
        </SimpleForm>
    </Edit>
);

export const ProductCreate = (props) => (
    <Create {...props}>
        <SimpleForm>
        <ReferenceInput label="Client" source="clientId" reference="clients">
                <SelectInput optionText="description" />
            </ReferenceInput>
            <TextInput source="description" />
            <TextInput source="label" />
            <BooleanInput defaultValue="true" label="Is Active" source="isActive" />
        </SimpleForm>
    </Create>
);