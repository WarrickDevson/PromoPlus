// in src/Promotions.js
import React from 'react';
import { List, Datagrid,TextField  } from 'react-admin';

export const StockCountList = (props) => (
    <List title="Stock Count List" {...props}>
        <Datagrid>
            <TextField label ="Client" source="promotionProduct.promotion.client.description"></TextField>
            <TextField label ="Location" source="promotionProduct.promotion.location.label"></TextField>  
            <TextField label ="Product" source="promotionProduct.product.label"></TextField>  
            <TextField source="count" /> 
            <TextField label ="Promoter" source="promoter.name"></TextField> 
            <TextField label ="Modified User" source="modifiedUser.name"></TextField>
        </Datagrid>
    </List>
);