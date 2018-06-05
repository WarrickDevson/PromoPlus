// in src/Promotions.js
import React from 'react';
import { List, Edit, Create, Datagrid,ArrayField,ReferenceArrayInput,SingleFieldList,ChipField, BooleanInput ,  EditButton,  SimpleForm, BooleanField,DateInput,DateField,ReferenceInput,SelectArrayInput ,SelectInput  } from 'react-admin';

const ModifiedUser = ({ record }) => {
    return <span>{record.modifiedUser ? `${record.modifiedUser.name + ' ' + record.modifiedUser.surname}` : ''}</span>;
};

const ClientField = ({ record }) => {
    return <span>{record.client ? `${record.client.description}` : ''}</span>;
};

const LocationField = ({ record }) => {
    return <span>{record.location ? `${record.location.label}` : ''}</span>;
};

const PromotionDescription = ({ record }) => {
    console.log(record)
    return <span>{record.client ? `${record.client.description} - ${record.location.label}` : ''}</span>;
};

export const PromotionList = (props) => (
    <List {...props}>
        <Datagrid>
            <ClientField label="Client"></ClientField>
            <LocationField label="Location"></LocationField>
            <DateField source="date" />
            <ArrayField  label="Products" source="promotionProduct">
                <SingleFieldList>
                    <ChipField source="product.label" />
                </SingleFieldList>
            </ArrayField >
            <ArrayField  label="Promoters" source="promotionPromoter">
                <SingleFieldList>
                    <ChipField source="promoter.name" />
                </SingleFieldList>
            </ArrayField >
            <BooleanField label="Is Active" source="isActive" />
            <ModifiedUser label="Modified User"></ModifiedUser>
            <EditButton />
        </Datagrid>
    </List>
);


export const PromotionEdit = (props) => (
    <Edit title={<PromotionDescription />} {...props}>
        <SimpleForm>
        <ReferenceInput label="Client" source="clientId" reference="clients">
                <SelectInput optionText="description" />
            </ReferenceInput>
            <ReferenceInput label="Location" source="locationId" reference="locations">
                <SelectInput optionText="label" />
            </ReferenceInput>
            <DateInput source="date" />
            <ReferenceArrayInput label="Products" source="productIds" reference="products">
                <SelectArrayInput optionText="label" />
            </ReferenceArrayInput>
            <ReferenceArrayInput label="Promoters" source="promoterIds" reference="promoters">
                <SelectArrayInput optionText="name" />
            </ReferenceArrayInput>
            <BooleanInput defaultValue="true" label="Is Active" source="isActive" />
        </SimpleForm>
    </Edit>
);


export const PromotionCreate = (props) => (
    <Create {...props}>
        <SimpleForm>
            <ReferenceInput label="Client" source="clientId" reference="clients">
                <SelectInput optionText="description" />
            </ReferenceInput>
            <ReferenceInput label="Location" source="locationId" reference="locations">
                <SelectInput optionText="label" />
            </ReferenceInput>
            <DateInput source="date" />
            <ReferenceArrayInput label="Products" source="productIds" reference="products">
                <SelectArrayInput optionText="label" />
            </ReferenceArrayInput>
            <ReferenceArrayInput label="Promoters" source="promoterIds" reference="promoters">
                <SelectArrayInput optionText="name" />
            </ReferenceArrayInput>
            <BooleanInput defaultValue="true" label="Is Active" source="isActive" />
        </SimpleForm>
    </Create>
);