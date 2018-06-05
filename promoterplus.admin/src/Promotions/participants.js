// in src/Participants.js
import React from 'react';
import { List,  TextField, Datagrid  } from 'react-admin';

export const ParticipantList = (props) => (
    <List {...props}>
        <Datagrid>
        <TextField label ="Type" source="participantType.description"></TextField>
        <TextField label ="Client" source="promotion.client.description"></TextField>
        <TextField label ="Location" source="promotion.location.label"></TextField>  
        <TextField label ="Product" source="product.label"></TextField> 
        <TextField label ="Age" source="age.description"></TextField>
        <TextField label ="Buying Power" source="buyingPower.description"></TextField> 
        <TextField label ="Feedback" source="feedback.description"></TextField> 
        <TextField label ="Gender" source="gender.description"></TextField> 
        <TextField label ="Race" source="race.description"></TextField>  
        <TextField label ="Repetition Type" source="repetitionType.description"></TextField> 
        <TextField label ="Time" source="startTime"></TextField> 
        <TextField label ="Promoter" source="promoter.name"></TextField> 
        <TextField label ="Modified User" source="modifiedUser.name"></TextField>
        </Datagrid>
    </List>
);
