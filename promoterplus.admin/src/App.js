import React from 'react';
import Menu from './menu';
import { fetchUtils, Admin, Resource } from 'react-admin';
import {API_ROOT} from './api-config';
import Dashboard from './dashboard';
import {LookupList, LookupCreate, LookupEdit} from './Lookups/lookup'
import {ProductList, ProductCreate, ProductEdit} from './Promotions/products'
import {PromoterList, PromoterCreate, PromoterEdit} from './Promotions/promoters'
import {LocationList, LocationCreate, LocationEdit} from './Promotions/locations'
import {PromotionList, PromotionCreate, PromotionEdit} from './Promotions/promotions'
import {StockCountList} from './Promotions/stockcounts'
import {ParticipantList} from './Promotions/participants'
import {TrafficList} from './Promotions/traffic'
import authProvider from './authProvider';
import jsonServerProvider from 'ra-data-json-server';

const httpClient = (url, options = {}) => {
  if (!options.headers) {
      options.headers = new Headers({ Accept: 'application/json' });
  }
  const token = localStorage.getItem('token');
  options.headers.set('Authorization', `Bearer ${token}`);
  return fetchUtils.fetchJson(url, options);
}
const dataProvider = jsonServerProvider(API_ROOT, httpClient);

const App = () => (
  <Admin title="Promoter Plus Admin" dashboard={Dashboard} dataProvider={dataProvider} authProvider={authProvider}>
    {/* <Resource name="ages" create={LookupCreate} edit={LookupEdit} list={LookupList} options={{ label: 'Ages' }}/>
    <Resource name="buyingpowers" create={LookupCreate} edit={LookupEdit} list={LookupList} options={{ label: 'Buying Powers' }}/>
    <Resource name="feedbacks" create={LookupCreate} edit={LookupEdit} list={LookupList} options={{ label: 'Feedbacks' }}/>
    <Resource name="genders" create={LookupCreate} edit={LookupEdit} list={LookupList} options={{ label: 'Genders' }}/>
    <Resource name="participanttypes" create={LookupCreate} edit={LookupEdit} list={LookupList} options={{ label: 'Participant Types' }}/>
    <Resource name="races" create={LookupCreate} edit={LookupEdit} list={LookupList} options={{ label: 'Races' }}/>
    <Resource name="repetitiontypes" create={LookupCreate} edit={LookupEdit} list={LookupList} options={{ label: 'Repetition Types' }}/> */}

    <Resource name="ages"/>
    <Resource name="buyingpowers"/>
    <Resource name="feedbacks"/>
    <Resource name="genders"/>
    <Resource name="participanttypes"/>
    <Resource name="races"/>
    <Resource name="repetitiontypes"/>

    <Resource name="clients" create={LookupCreate} edit={LookupEdit} list={LookupList} options={{ label: 'Clients' }}/>
    <Resource name="promotions" create={PromotionCreate} edit={PromotionEdit} list={PromotionList} options={{ label: 'Promotions' }}/>
    <Resource name="products" create={ProductCreate} edit={ProductEdit} list={ProductList} options={{ label: 'Products' }}/>
    <Resource name="locations" create={LocationCreate} edit={LocationEdit} list={LocationList} options={{ label: 'Locations' }}/>
    <Resource name="promoters" create={PromoterCreate} edit={PromoterEdit} list={PromoterList} options={{ label: 'Promoters' }}/>
    <Resource name="stockcounts" list={StockCountList} options={{ label: 'Stock Count' }}/>
    <Resource name="participants" list={ParticipantList} options={{ label: 'Participants' }}/>
    <Resource name="traffic" list={TrafficList} options={{ label: 'Traffic' }}/>
  </Admin>
);

export default App;