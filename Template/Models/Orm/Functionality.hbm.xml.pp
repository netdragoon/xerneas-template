<?xml version="1.0" encoding="utf-8"?>

<hibernate-mapping xmlns="urn:nhibernate-mapping-2.2">
  <class name="$rootnamespace$.Models.Functionality, $rootnamespace$" table="Functionalities">
    <id name="Id">
      <generator class="identity" />
    </id>

    <many-to-one name="Module" column="ModuleId" />

    <property name="Name" length="100" not-null="true" />
    <property name="Description" length="100" />
    <property name="Action" length="100" not-null="true" index="idx_action" />
    <property name="Controller" length="100" not-null="true" index="idx_controller"  />
    <property name="HttpMethod" length="100" not-null="true" index="idx_httpmethod" />
    <property name="Default" column="DefaultFunctionality" not-null="true" />
    <property name="Created" not-null="true" />
    <property name="Updated" not-null="true" />
  </class>
</hibernate-mapping>