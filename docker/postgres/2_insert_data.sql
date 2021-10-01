--
-- PostgreSQL database dump
--

-- Dumped from database version 13.3
-- Dumped by pg_dump version 13.3

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

--
-- Data for Name: AspNetGroups; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public."AspNetGroups" VALUES ('138f136b-e5c1-4eaa-b61d-529a081fd435', 'Super Admins', 'Administrators with super privileges');
INSERT INTO public."AspNetGroups" VALUES ('6097810b-e070-4514-affa-35e76cc84d02', 'Admins', 'Administrators');


--
-- Data for Name: AspNetRoles; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public."AspNetRoles" VALUES ('4c11fd92-3871-4684-ba6a-57ed1ebd77a7', 'Administrator', 'admin', 'ADMIN', 'f869fe85-8578-4ced-a419-60efd11b1aaf');


--
-- Data for Name: AspNetRoleClaims; Type: TABLE DATA; Schema: public; Owner: postgres
--



--
-- Data for Name: AspNetUsers; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public."AspNetUsers" VALUES ('c6b9b260-9d81-457d-9897-6495f68bb34d', 'Super Admin', 'super', 'SUPER', 'super@abacuza.com', 'SUPER@ABACUZA.COM', false, 'AQAAAAEAACcQAAAAEOjRegiOfzVJqdZtZZfAB80OdNFWxXIBrLQ/8rv8Uhq2N6LMWwSLH/ZMKzn1Ioig4g==', 'M2QPQQWSS5NNMXOVE5CV3NWP7M4T7UYL', '19f80875-9c07-4b5b-808c-969fca9984c3', NULL, false, false, NULL, true, 0);
INSERT INTO public."AspNetUsers" VALUES ('2e5832e1-d112-4369-9742-b394f57930b9', 'Abacuza User', 'abacuza', 'ABACUZA', 'abacuza@abacuza.com', 'ABACUZA@ABACUZA.COM', false, 'AQAAAAEAACcQAAAAEHiCkTWGxyvIBDoshUPBF6aN/Ec59QlH4Z+gR6X6HkZFGIZpq/yFCd8WB90vnT/rcg==', 'XVHOYY62IS4Q5ESBCJLJZ34XN7NY3R4P', '88b3f45a-e57b-4200-bc4e-f4dde30a7cc3', NULL, false, false, NULL, true, 0);


--
-- Data for Name: AspNetUserClaims; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public."AspNetUserClaims" VALUES (1, 'c6b9b260-9d81-457d-9897-6495f68bb34d', 'name', 'super');
INSERT INTO public."AspNetUserClaims" VALUES (2, 'c6b9b260-9d81-457d-9897-6495f68bb34d', 'nickname', 'Super Admin');
INSERT INTO public."AspNetUserClaims" VALUES (3, 'c6b9b260-9d81-457d-9897-6495f68bb34d', 'email', 'super@abacuza.com');
INSERT INTO public."AspNetUserClaims" VALUES (4, 'c6b9b260-9d81-457d-9897-6495f68bb34d', 'role', 'admin');
INSERT INTO public."AspNetUserClaims" VALUES (5, '2e5832e1-d112-4369-9742-b394f57930b9', 'name', 'abacuza');
INSERT INTO public."AspNetUserClaims" VALUES (6, '2e5832e1-d112-4369-9742-b394f57930b9', 'nickname', 'Abacuza User');
INSERT INTO public."AspNetUserClaims" VALUES (7, '2e5832e1-d112-4369-9742-b394f57930b9', 'email', 'abacuza@abacuza.com');


--
-- Data for Name: AspNetUserGroups; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public."AspNetUserGroups" VALUES ('2e5832e1-d112-4369-9742-b394f57930b9', '6097810b-e070-4514-affa-35e76cc84d02');
INSERT INTO public."AspNetUserGroups" VALUES ('c6b9b260-9d81-457d-9897-6495f68bb34d', '138f136b-e5c1-4eaa-b61d-529a081fd435');


--
-- Data for Name: AspNetUserLogins; Type: TABLE DATA; Schema: public; Owner: postgres
--



--
-- Data for Name: AspNetUserRoles; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public."AspNetUserRoles" VALUES ('c6b9b260-9d81-457d-9897-6495f68bb34d', '4c11fd92-3871-4684-ba6a-57ed1ebd77a7');


--
-- Data for Name: AspNetUserTokens; Type: TABLE DATA; Schema: public; Owner: postgres
--



--
-- Data for Name: DeviceCodes; Type: TABLE DATA; Schema: public; Owner: postgres
--



--
-- Data for Name: PersistedGrants; Type: TABLE DATA; Schema: public; Owner: postgres
--



--
-- Data for Name: __EFMigrationsHistory; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public."__EFMigrationsHistory" VALUES ('20210327114719_InitialCreate', '5.0.4');
INSERT INTO public."__EFMigrationsHistory" VALUES ('20210327114815_InitialCreate', '5.0.4');
INSERT INTO public."__EFMigrationsHistory" VALUES ('20210925032847_20210925_AddUserGroup', '5.0.9');


--
-- Data for Name: qrtz_job_details; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.qrtz_job_details VALUES ('QuartzScheduler', 'job-update-executor', 'job-execution', NULL, 'Abacuza.Jobs.ApiService.Models.JobUpdateExecutor, Abacuza.Jobs.ApiService', false, true, false, false, NULL);


--
-- Data for Name: qrtz_triggers; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.qrtz_triggers VALUES ('QuartzScheduler', 'job-update-executor-trigger', 'job-execution', 'job-update-executor', 'job-execution', NULL, 637681693635789238, 637681693485789238, 5, 'ACQUIRED', 'SIMPLE', 637681677585789238, NULL, NULL, -1, NULL);


--
-- Data for Name: qrtz_blob_triggers; Type: TABLE DATA; Schema: public; Owner: postgres
--



--
-- Data for Name: qrtz_calendars; Type: TABLE DATA; Schema: public; Owner: postgres
--



--
-- Data for Name: qrtz_cron_triggers; Type: TABLE DATA; Schema: public; Owner: postgres
--



--
-- Data for Name: qrtz_fired_triggers; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.qrtz_fired_triggers VALUES ('QuartzScheduler', 'daxnet-ThinkPad-E450637681677562147273637681677589029402', 'job-update-executor-trigger', 'job-execution', 'daxnet-ThinkPad-E450637681677562147273', 637681693486138355, 637681693635789238, 5, 'ACQUIRED', NULL, NULL, false, false);


--
-- Data for Name: qrtz_locks; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.qrtz_locks VALUES ('QuartzScheduler', 'TRIGGER_ACCESS');
INSERT INTO public.qrtz_locks VALUES ('QuartzScheduler', 'STATE_ACCESS');


--
-- Data for Name: qrtz_paused_trigger_grps; Type: TABLE DATA; Schema: public; Owner: postgres
--



--
-- Data for Name: qrtz_scheduler_state; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.qrtz_scheduler_state VALUES ('QuartzScheduler', 'daxnet-ThinkPad-E450637681677562147273', 637681693567692340, 7500);


--
-- Data for Name: qrtz_simple_triggers; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.qrtz_simple_triggers VALUES ('QuartzScheduler', 'job-update-executor-trigger', 'job-execution', -1, 15000, 107);


--
-- Data for Name: qrtz_simprop_triggers; Type: TABLE DATA; Schema: public; Owner: postgres
--



--
-- Name: AspNetRoleClaims_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public."AspNetRoleClaims_Id_seq"', 1, false);


--
-- Name: AspNetUserClaims_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public."AspNetUserClaims_Id_seq"', 7, true);


--
-- PostgreSQL database dump complete
--

